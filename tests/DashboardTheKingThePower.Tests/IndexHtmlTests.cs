namespace DashboardTheKingThePower.Tests;

/// <summary>
/// Guarda contra regressão do pipeline de CSS/JS: o projeto deve compilar Tailwind
/// localmente (CI roda npm run build:css) e servir Sortable versionado — nunca CDN.
/// </summary>
public class IndexHtmlTests
{
    private static string FindRepoFile(params string[] parts)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine([dir.FullName, .. parts]);
            if (File.Exists(candidate)) return candidate;
            dir = dir.Parent;
        }
        throw new FileNotFoundException("arquivo nao encontrado: " + Path.Combine(parts));
    }

    private static string IndexHtml() =>
        File.ReadAllText(FindRepoFile("src", "DashboardTheKingThePower", "wwwroot", "index.html"));

    [Test]
    public void IndexHtml_NaoReferenciaCdn()
    {
        var html = IndexHtml();
        Assert.That(html, Does.Not.Contain("cdn.tailwindcss.com"));
        Assert.That(html, Does.Not.Contain("cdn.jsdelivr.net"));
    }

    [Test]
    public void IndexHtml_UsaCssEBibliotecaLocais()
    {
        var html = IndexHtml();
        Assert.That(html, Does.Contain("href=\"css/tailwind.css\""));
        Assert.That(html, Does.Contain("src=\"lib/sortable.min.js\""));
    }

    [Test]
    public void TailwindInput_ImportaTailwindV4()
    {
        var input = File.ReadAllText(
            FindRepoFile("src", "DashboardTheKingThePower", "css", "tailwind.input.css"));
        Assert.That(input, Does.Contain("@import \"tailwindcss\""));
    }

    /// <summary>
    /// Todo JS do index deve ser arquivo externo (js/theme.js, js/boot.js, lib/,
    /// _framework) — a CSP do nginx mitiga XSS limitando script-src a origem propria.
    /// Unica excecao inline tolerada: o <script type="importmap"></script> VAZIO,
    /// que o build do Blazor preenche fora do HTML estatico. Remover o elemento
    /// quebra o boot (o loader resolve dotnet.js via import map).
    /// </summary>
    [Test]
    public void IndexHtml_NaoTemScriptsInline_PraCspEstrita()
    {
        var html = IndexHtml();
        Assert.That(html, Does.Contain("<script type=\"importmap\"></script>"));
        // <script> sem atributo src e com conteudo (o importmap vazio nao conta)
        Assert.That(html, Does.Not.Match(@"<script(?![^>]*\bsrc\s*=)[^>]*>\s*[^<\s]"));
    }
}

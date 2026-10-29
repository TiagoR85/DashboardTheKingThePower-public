using DashboardTheKingThePower.Models;
using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class FaqFilterTests
{
    private static List<FaqItem> Sample() => new()
    {
        new() { Id = "1", Question = "Como faço login?", Answer = "Use email e senha na tela de acesso.", Category = "Conta" },
        new() { Id = "2", Question = "Como troco o tema?", Answer = "Menu superior, ícone de lua.", Category = "Interface" },
        new() { Id = "3", Question = "Esqueci minha senha", Answer = "Clique em recuperar senha na tela de acesso.", Category = "Conta" },
    };

    [Test]
    public void Filter_Vazio_RetornaTodos()
        => Assert.That(FaqFilter.Filter(Sample(), ""), Has.Count.EqualTo(3));

    [Test]
    public void Filter_PorPergunta_IgnoraMaiusculas()
        => Assert.That(FaqFilter.Filter(Sample(), "LOGIN"), Has.Count.EqualTo(1));

    [Test]
    public void Filter_PorResposta_TambemBusca()
        => Assert.That(FaqFilter.Filter(Sample(), "lua"), Has.Count.EqualTo(1));

    [Test]
    public void Filter_SemMatch_ListaVazia()
        => Assert.That(FaqFilter.Filter(Sample(), "zzz"), Is.Empty);

    // Dívida: ramos nulo/espaços do guard query?.Trim() ?? "".
    [Test]
    public void Filter_QueryNula_RetornaTodos()
        => Assert.That(FaqFilter.Filter(Sample(), null!), Has.Count.EqualTo(3));

    [Test]
    public void Filter_QuerySoEspacos_RetornaTodos()
        => Assert.That(FaqFilter.Filter(Sample(), "   "), Has.Count.EqualTo(3));
}

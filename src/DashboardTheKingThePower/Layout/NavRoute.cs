namespace DashboardTheKingThePower.Layout;

/// <summary>
/// Regra de rota ativa da navegação inferior (espelha o NavClass do Sidebar):
/// "/" só casa na raiz exata; demais rotas casam por prefixo, para que
/// atalhos acendam nas páginas de detalhe (ex.: /orders/{Id} → "Pedidos").
/// </summary>
public static class NavRoute
{
    public static bool IsActive(string currentPath, string href)
        => href == "/"
            ? string.Equals(currentPath, "/", StringComparison.OrdinalIgnoreCase)
            : currentPath.StartsWith(href, StringComparison.OrdinalIgnoreCase);
}

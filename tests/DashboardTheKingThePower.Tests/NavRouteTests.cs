using DashboardTheKingThePower.Layout;

namespace DashboardTheKingThePower.Tests;

public class NavRouteTests
{
    [Test]
    public void Raiz_AtivaApenasNaRaizExata()
    {
        Assert.That(NavRoute.IsActive("/", "/"), Is.True);
        Assert.That(NavRoute.IsActive("/orders", "/"), Is.False);
        Assert.That(NavRoute.IsActive("/analytics", "/"), Is.False);
    }

    [Test]
    public void RotaExata_Ativa()
    {
        Assert.That(NavRoute.IsActive("/orders", "/orders"), Is.True);
        Assert.That(NavRoute.IsActive("/analytics", "/analytics"), Is.True);
        Assert.That(NavRoute.IsActive("/products", "/products"), Is.True);
    }

    [Test]
    public void RotaDeDetalho_AtivaAtalhoDaLista()
    {
        // Fase 3: /orders/{Id} e /products/{Id} devem acender o atalho da listagem
        Assert.That(NavRoute.IsActive("/orders/ORD-20261006-ab12cd", "/orders"), Is.True);
        Assert.That(NavRoute.IsActive("/products/PROD-001", "/products"), Is.True);
    }

    [Test]
    public void RotaIrrelacionada_NaoAtivaOutrosAtalhos()
    {
        Assert.That(NavRoute.IsActive("/wishlist", "/orders"), Is.False);
        Assert.That(NavRoute.IsActive("/orders", "/analytics"), Is.False);
        Assert.That(NavRoute.IsActive("/ui/buttons", "/products"), Is.False);
    }

    [Test]
    public void CaseInsensitive_MantidoDoComportamentoAnterior()
    {
        Assert.That(NavRoute.IsActive("/Orders", "/orders"), Is.True);
        Assert.That(NavRoute.IsActive("/Orders/ORD-1", "/orders"), Is.True);
    }
}

using DashboardTheKingThePower.Models;
using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class CartServiceTests
{
    private static CartItem Item(string id, decimal price, int qty = 1)
        => new() { ProductId = id, Name = $"Item {id}", Price = price, Qty = qty };

    [Test]
    public void Add_ItemNovo_EntraComQty1()
    {
        var cart = CartService.Add(new(), Item("P1", 50m));
        Assert.That(cart, Has.Count.EqualTo(1));
        Assert.That(cart[0].Qty, Is.EqualTo(1));
    }

    [Test]
    public void Add_ItemRepetido_IncrementaQuantidade()
    {
        var cart = CartService.Add(new() { Item("P1", 50m, 2) }, Item("P1", 50m));
        Assert.That(cart, Has.Count.EqualTo(1));
        Assert.That(cart[0].Qty, Is.EqualTo(3));
    }

    [Test]
    public void SetQty_AbaixoDe1_ParaEm1()
    {
        var cart = CartService.SetQty(new() { Item("P1", 50m, 3) }, "P1", 0);
        Assert.That(cart[0].Qty, Is.EqualTo(1));
    }

    [Test]
    public void Remove_ItemExcluido()
    {
        var cart = CartService.Remove(new() { Item("P1", 50m), Item("P2", 30m) }, "P1");
        Assert.That(cart.Select(i => i.ProductId), Is.EquivalentTo(new[] { "P2" }));
    }

    [Test]
    public void CalculateTotals_CarrinhoVazio_TudoZero()
    {
        var t = CartService.CalculateTotals(new(), false);
        Assert.That(t.Subtotal, Is.EqualTo(0m));
        Assert.That(t.Frete, Is.EqualTo(0m));
        Assert.That(t.Total, Is.EqualTo(0m));
    }

    [Test]
    public void CalculateTotals_ItemZeroExpress_Frete2490()
    {
        var t = CartService.CalculateTotals(new() { Item("P1", 0m) }, true);
        Assert.That(t.Subtotal, Is.EqualTo(0m));
        Assert.That(t.Frete, Is.EqualTo(24.90m));
        Assert.That(t.Total, Is.EqualTo(24.90m));
    }

    [Test]
    public void CalculateTotals_ItemZeroPadrao_Frete1490()
    {
        var t = CartService.CalculateTotals(new() { Item("P1", 0m) }, false);
        Assert.That(t.Frete, Is.EqualTo(14.90m));
    }

    [Test]
    public void CalculateTotals_PadraoAbaixoDe200_Frete1490()
    {
        var t = CartService.CalculateTotals(new() { Item("P1", 100m) }, false);
        Assert.That(t.Subtotal, Is.EqualTo(100m));
        Assert.That(t.Frete, Is.EqualTo(14.90m));
        Assert.That(t.Total, Is.EqualTo(114.90m));
    }

    [Test]
    public void CalculateTotals_PadraoAcimaDe200_FreteGratis()
    {
        var t = CartService.CalculateTotals(new() { Item("P1", 250m) }, false);
        Assert.That(t.Frete, Is.EqualTo(0m));
    }

    [Test]
    public void CalculateTotals_Express_Frete2490_Sempre()
    {
        var t = CartService.CalculateTotals(new() { Item("P1", 500m) }, true);
        Assert.That(t.Frete, Is.EqualTo(24.90m));
    }

    [Test]
    public void Remove_ItemInexistente_NaoAlteraCarrinho()
    {
        var cart = CartService.Remove(new() { Item("P1", 50m), Item("P2", 30m) }, "PX");
        Assert.That(cart, Has.Count.EqualTo(2));
    }

    [Test]
    public void SetQty_Negativo_ParaEm1()
    {
        var cart = CartService.SetQty(new() { Item("P1", 50m, 3) }, "P1", -5);
        Assert.That(cart[0].Qty, Is.EqualTo(1));
    }
}

using DashboardTheKingThePower.Models;
using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class OrderServiceTests
{
    [Test]
    public void Build_CopiaItensECalculaTotais()
    {
        var items = new List<CartItem>
        {
            new() { ProductId = "P1", Name = "Item 1", Price = 100m, Qty = 2 },
            new() { ProductId = "P2", Name = "Item 2", Price = 50m, Qty = 1 },
        };

        var order = OrderService.Build(items, "Maria Silva", express: false);

        Assert.That(order.Items, Has.Count.EqualTo(2));
        Assert.That(order.Subtotal, Is.EqualTo(250m));
        Assert.That(order.Frete, Is.EqualTo(0m));      // ≥ 200 → grátis
        Assert.That(order.Total, Is.EqualTo(250m));
        Assert.That(order.Customer, Is.EqualTo("Maria Silva"));
        Assert.That(order.Status, Is.EqualTo("Pendente"));
        Assert.That(order.Id, Does.StartWith($"ORD-{DateTime.Now:yyyyMMdd}-"));
    }

    [Test]
    public void Build_Express_AdicionaFrete()
    {
        var items = new List<CartItem> { new() { ProductId = "P1", Name = "X", Price = 100m, Qty = 1 } };
        var order = OrderService.Build(items, "João", express: true);
        Assert.That(order.Frete, Is.EqualTo(24.90m));
        Assert.That(order.Total, Is.EqualTo(124.90m));
    }

    [Test]
    public void Build_CarrinhoVazio_TotaisZerados()
    {
        var order = OrderService.Build(new(), "Ana", false);
        Assert.That(order.Total, Is.EqualTo(0m));
        Assert.That(order.Items, Is.Empty);
    }
}

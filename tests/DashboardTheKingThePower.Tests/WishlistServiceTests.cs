using DashboardTheKingThePower.Models;
using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class WishlistServiceTests
{
    private static WishlistItem Item(string id) => new() { ProductId = id, Name = $"Item {id}", Price = 10m };

    [Test]
    public void Add_NovoItem_RetornaTrueEEntra()
    {
        var list = new List<WishlistItem>();
        Assert.That(WishlistService.Add(list, Item("P1")), Is.True);
        Assert.That(list, Has.Count.EqualTo(1));
    }

    [Test]
    public void Add_Duplicado_RetornaFalseENaoRepete()
    {
        var list = new List<WishlistItem> { Item("P1") };
        Assert.That(WishlistService.Add(list, Item("P1")), Is.False);
        Assert.That(list, Has.Count.EqualTo(1));
    }

    [Test]
    public void Remove_Existente_RetornaTrueESai()
    {
        var list = new List<WishlistItem> { Item("P1"), Item("P2") };
        Assert.That(WishlistService.Remove(list, "P1"), Is.True);
        Assert.That(list.Select(i => i.ProductId), Is.EquivalentTo(new[] { "P2" }));
    }

    [Test]
    public void Remove_Inexistente_RetornaFalse()
    {
        var list = new List<WishlistItem> { Item("P1") };
        Assert.That(WishlistService.Remove(list, "PX"), Is.False);
    }
}

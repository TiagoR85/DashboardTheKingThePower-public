using DashboardTheKingThePower.Models;
using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class ProductServiceTests
{
    private static Product P(string id) => new() { Id = id, Name = $"Item {id}" };

    // Dívida da revisão final: CreateProduct usava Last().Id+1 (ordem-dependent);
    // NextId = max+1 (mesmo padrão aprovado p/ Contato na Task 3).
    [Test]
    public void NextId_ListaVazia_RetornaProd001()
        => Assert.That(ProductService.NextId(new()), Is.EqualTo("PROD-001"));

    [Test]
    public void NextId_SeedCompleta_RetornaProximo()
    {
        var products = Enumerable.Range(1, 12).Select(i => P($"PROD-{i:D3}")).ToList();
        Assert.That(ProductService.NextId(products), Is.EqualTo("PROD-013"));
    }

    [Test]
    public void NextId_ListaDesordenada_PegaMaiorNaoUltimo()
    {
        var products = new List<Product> { P("PROD-003"), P("PROD-001") };
        Assert.That(ProductService.NextId(products), Is.EqualTo("PROD-004"));
    }
}

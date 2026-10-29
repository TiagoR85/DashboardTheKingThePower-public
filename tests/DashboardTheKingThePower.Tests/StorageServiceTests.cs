using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class StorageServiceTests
{
    private sealed class Thing
    {
        public string Name { get; set; } = "";
    }

    private static StorageService<Thing> Store(FakeLocalStorage fake, string key, Func<List<Thing>> seed)
        => new(fake, key, seed);

    [Test]
    public async Task LoadAsync_SemDado_PopulaESalvaSeed()
    {
        var fake = new FakeLocalStorage();
        var seedCalls = 0;
        var store = Store(fake, "dtkp:test", () =>
        {
            seedCalls++;
            return new List<Thing> { new() { Name = "seed" } };
        });

        var items = await store.LoadAsync();

        Assert.That(seedCalls, Is.EqualTo(1));
        Assert.That(items, Has.Count.EqualTo(1));
        Assert.That(items[0].Name, Is.EqualTo("seed"));
        Assert.That(fake.Store, Contains.Key("dtkp:test")); // persistiu na 1ª carga
    }

    [Test]
    public async Task LoadAsync_ComDado_NaoChamaSeed()
    {
        var fake = new FakeLocalStorage();
        var store = Store(fake, "dtkp:pre", () => throw new InvalidOperationException("seed nao deve rodar"));
        await store.SaveAsync(new List<Thing> { new() { Name = "salvo" } });

        var items = await store.LoadAsync();

        Assert.That(items, Has.Count.EqualTo(1));
        Assert.That(items[0].Name, Is.EqualTo("salvo"));
    }

    [Test]
    public async Task SaveAsync_PersisteNaChaveDoConstructor()
    {
        var fake = new FakeLocalStorage();
        var store = Store(fake, "dtkp:chave-exata", () => new());

        await store.SaveAsync(new List<Thing> { new() { Name = "x" } });

        Assert.That(fake.Store, Contains.Key("dtkp:chave-exata"));
    }
}

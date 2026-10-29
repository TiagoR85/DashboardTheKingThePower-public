using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class UserServiceTests
{
    // Dívida da revisão final: UpdateProfileAsync sem teste (ramo existente/lançamento).
    [Test]
    public async Task UpdateProfileAsync_UsuarioExistente_AtualizaEPersiste()
    {
        var service = new UserService(new FakeLocalStorage());
        var user = await service.CreateUserAsync("Ana", "ana@exemplo.com", "Senha#123");

        var updated = await service.UpdateProfileAsync(user.Id, "Ana Silva", "ana.nova@exemplo.com");

        Assert.That(updated.Name, Is.EqualTo("Ana Silva"));
        Assert.That(updated.Email, Is.EqualTo("ana.nova@exemplo.com"));

        // Persistência real: reload do storage acha pelo NOVO email com a MESMA senha.
        var reloaded = await service.ValidatePasswordAsync("ana.nova@exemplo.com", "Senha#123");
        Assert.That(reloaded, Is.Not.Null);
        Assert.That(reloaded!.Name, Is.EqualTo("Ana Silva"));
    }

    [Test]
    public void UpdateProfileAsync_UsuarioInexistente_LancaInvalidOperation()
    {
        var service = new UserService(new FakeLocalStorage());
        Assert.ThrowsAsync<InvalidOperationException>(
            () => service.UpdateProfileAsync("id-inexistente", "X", "x@exemplo.com"));
    }
}

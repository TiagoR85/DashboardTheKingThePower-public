using DashboardTheKingThePower.Models;
using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class ContactServiceTests
{
    private static List<Contact> Sample() => new()
    {
        new() { Id = "C1", Name = "Ana Souza", Email = "ana@ex.com", Company = "Souza Ltda", Status = "Ativo" },
        new() { Id = "C2", Name = "Bruno Lima", Email = "bruno@ex.com", Company = "Lima Eng", Status = "Lead" },
        new() { Id = "C3", Name = "Carla Dias", Email = "carla@ex.com", Company = "Dias SA", Status = "Ativo" },
    };

    [Test]
    public void Filter_QueryVazia_RetornaTodos()
        => Assert.That(ContactService.Filter(Sample(), "", ""), Has.Count.EqualTo(3));

    [Test]
    public void Filter_PorNome_IgnoraMaiusculas()
    {
        var result = ContactService.Filter(Sample(), "ana", "");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Ana Souza"));
    }

    [Test]
    public void Filter_PorEmpresa_TambemBusca()
        => Assert.That(ContactService.Filter(Sample(), "Lima Eng", ""), Has.Count.EqualTo(1));

    [Test]
    public void Filter_PorStatus()
    {
        var result = ContactService.Filter(Sample(), "", "Lead");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo("C2"));
    }

    [Test]
    public void Filter_QueryEStatus_Combinados()
        => Assert.That(ContactService.Filter(Sample(), "a", "Ativo"), Has.Count.EqualTo(2));

    [Test]
    public void Filter_SemResultado_ListaVazia()
        => Assert.That(ContactService.Filter(Sample(), "inexistente", ""), Is.Empty);

    [Test]
    public void NextId_AposExclusao_NaoColide()
    {
        var contacts = new List<Contact>
        {
            new() { Id = "CONT-001" }, new() { Id = "CONT-002" }, new() { Id = "CONT-003" },
            new() { Id = "CONT-004" }, new() { Id = "CONT-005" },
        };
        contacts.RemoveAll(c => c.Id == "CONT-003");
        Assert.That(ContactService.NextId(contacts), Is.EqualTo("CONT-006"));
    }

    [Test]
    public void NextId_ListaVazia_RetornaPrimeiroId()
        => Assert.That(ContactService.NextId(new List<Contact>()), Is.EqualTo("CONT-001"));

    [Test]
    public void NextId_SufixoNaoNumerico_IgnoraSemLancar()
    {
        var contacts = new List<Contact> { new() { Id = "CONT-ABC" }, new() { Id = "CONT-002" } };
        Assert.That(ContactService.NextId(contacts), Is.EqualTo("CONT-003"));
    }
}

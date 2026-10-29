using Blazored.LocalStorage;
using DashboardTheKingThePower.Models;

namespace DashboardTheKingThePower.Services;

public class ContactService
{
    private readonly StorageService<Contact> _store;

    public ContactService(ILocalStorageService localStorage)
        => _store = new StorageService<Contact>(localStorage, "dtkp:contacts", Seed);

    public Task<List<Contact>> GetAsync() => _store.LoadAsync();
    public Task SaveAsync(List<Contact> contacts) => _store.SaveAsync(contacts);

    /// <summary>Filtro puro: nome/email/empresa (case-insensitive) + status exato.</summary>
    public static List<Contact> Filter(List<Contact> contacts, string query, string status)
    {
        var q = query?.Trim() ?? "";
        return contacts
            .Where(c =>
                (string.IsNullOrEmpty(status) || c.Status == status) &&
                (q.Length == 0 ||
                 c.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                 c.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                 c.Company.Contains(q, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    /// <summary>Próximo ID livre: maior sufixo numérico +1 (nunca colide após exclusões).</summary>
    public static string NextId(List<Contact> contacts)
    {
        var max = 0;
        foreach (var c in contacts ?? new List<Contact>())
        {
            // Só interessa o sufixo após o último '-'; IDs malformados são ignorados.
            var idx = c.Id?.LastIndexOf('-') ?? -1;
            if (idx < 0 || idx == c.Id!.Length - 1) continue;
            if (int.TryParse(c.Id.AsSpan(idx + 1), out var n) && n > max)
                max = n;
        }
        return $"CONT-{(max + 1):D3}";
    }

    private static List<Contact> Seed() => new()
    {
        new() { Id = "CONT-001", Name = "Ana Souza", Email = "ana.souza@exemplo.com", Phone = "(11) 98765-4321", Company = "Souza Consultoria", Status = "Ativo" },
        new() { Id = "CONT-002", Name = "Bruno Lima", Email = "bruno.lima@exemplo.com", Phone = "(21) 99876-1234", Company = "Lima Engenharia", Status = "Lead" },
        new() { Id = "CONT-003", Name = "Carla Mendes", Email = "carla.mendes@exemplo.com", Phone = "(31) 98877-2233", Company = "Mendes & Filhos", Status = "Ativo" },
        new() { Id = "CONT-004", Name = "Diego Santos", Email = "diego.santos@exemplo.com", Phone = "(41) 99776-3344", Company = "Santos Logística", Status = "Inativo" },
        new() { Id = "CONT-005", Name = "Elisa Rocha", Email = "elisa.rocha@exemplo.com", Phone = "(51) 99665-4455", Company = "Rocha Design", Status = "Lead" },
    };
}

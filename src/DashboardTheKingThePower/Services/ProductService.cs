using Blazored.LocalStorage;
using DashboardTheKingThePower.Models;

namespace DashboardTheKingThePower.Services;

public class ProductService
{
    private readonly StorageService<Product> _store;

    public ProductService(ILocalStorageService localStorage)
        => _store = new StorageService<Product>(localStorage, "dtkp:products", Seed);

    public Task<List<Product>> GetAsync() => _store.LoadAsync();
    public Task SaveAsync(List<Product> products) => _store.SaveAsync(products);

    public static Product? Find(List<Product> products, string id)
        => products.FirstOrDefault(p => p.Id == id);

    /// <summary>
    /// Próximo id livre: maior sufixo numérico +1 — nunca depende da ordem da lista
    /// (substitui o antigo Last().Id+1 da UI; mesmo padrão aprovado p/ Contato na Task 3).
    /// </summary>
    public static string NextId(List<Product> products)
    {
        var max = products.Count == 0
            ? 0
            : products.Max(p => int.Parse(p.Id.Replace("PROD-", "")));
        return $"PROD-{(max + 1):D3}";
    }

    // Seed: cópia exata da lista inline que vivia em Pages/Products.razor (12 itens, dados inalterados)
    private static List<Product> Seed() => new()
    {
        new() { Id = "PROD-001", Name = "Wireless Bluetooth Headphones", Category = "Electronics", Price = 79.99m, Stock = 45 },
        new() { Id = "PROD-002", Name = "USB-C Charging Cable", Category = "Electronics", Price = 12.99m, Stock = 120 },
        new() { Id = "PROD-003", Name = "Mechanical Keyboard RGB", Category = "Electronics", Price = 149.99m, Stock = 8 },
        new() { Id = "PROD-004", Name = "Classic Fit Cotton T-Shirt", Category = "Clothing", Price = 24.99m, Stock = 85 },
        new() { Id = "PROD-005", Name = "Slim Fit Denim Jeans", Category = "Clothing", Price = 59.99m, Stock = 32 },
        new() { Id = "PROD-006", Name = "Running Shoes Pro", Category = "Sports", Price = 89.99m, Stock = 0 },
        new() { Id = "PROD-007", Name = "Yoga Mat Premium", Category = "Sports", Price = 34.99m, Stock = 55 },
        new() { Id = "PROD-008", Name = "Ceramic Coffee Mug Set", Category = "Home", Price = 19.99m, Stock = 200 },
        new() { Id = "PROD-009", Name = "LED Desk Lamp", Category = "Home", Price = 44.99m, Stock = 3 },
        new() { Id = "PROD-010", Name = "Stainless Steel Water Bottle", Category = "Sports", Price = 15.99m, Stock = 95 },
        new() { Id = "PROD-011", Name = "Wireless Mouse Ergonomic", Category = "Electronics", Price = 29.99m, Stock = 62 },
        new() { Id = "PROD-012", Name = "Linen Throw Pillow", Category = "Home", Price = 27.99m, Stock = 12 },
    };
}

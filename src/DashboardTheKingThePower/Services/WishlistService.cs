using Blazored.LocalStorage;
using DashboardTheKingThePower.Models;

namespace DashboardTheKingThePower.Services;

public class WishlistService
{
    private readonly StorageService<WishlistItem> _store;

    public WishlistService(ILocalStorageService localStorage)
        => _store = new StorageService<WishlistItem>(localStorage, "dtkp:wishlist", Seed);

    public Task<List<WishlistItem>> GetAsync() => _store.LoadAsync();
    public Task SaveAsync(List<WishlistItem> items) => _store.SaveAsync(items);

    public static bool Add(List<WishlistItem> items, WishlistItem item)
    {
        if (items.Any(i => i.ProductId == item.ProductId)) return false;
        items.Add(item);
        return true;
    }

    public static bool Remove(List<WishlistItem> items, string productId)
        => items.RemoveAll(i => i.ProductId == productId) > 0;

    // Seed alinhado ao catálogo de Products.razor (IDs/nome/preço reais)
    private static List<WishlistItem> Seed() => new()
    {
        new() { ProductId = "PROD-001", Name = "Wireless Bluetooth Headphones", Category = "Electronics", Price = 79.99m },
        new() { ProductId = "PROD-004", Name = "Classic Fit Cotton T-Shirt", Category = "Clothing", Price = 24.99m },
    };
}

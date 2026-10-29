using Blazored.LocalStorage;
using DashboardTheKingThePower.Models;

namespace DashboardTheKingThePower.Services;

public record CartTotals(decimal Subtotal, decimal Frete, decimal Total);

public class CartService
{
    private readonly StorageService<CartItem> _store;

    public CartService(ILocalStorageService localStorage)
        => _store = new StorageService<CartItem>(localStorage, "dtkp:cart", () => new());

    public Task<List<CartItem>> GetAsync() => _store.LoadAsync();
    public Task SaveAsync(List<CartItem> items) => _store.SaveAsync(items);
    public Task ClearAsync() => _store.SaveAsync(new List<CartItem>());

    public static List<CartItem> Add(List<CartItem> items, CartItem item)
    {
        var existing = items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existing != null) existing.Qty += item.Qty;
        else items.Add(item);
        return items;
    }

    public static List<CartItem> SetQty(List<CartItem> items, string productId, int qty)
    {
        var item = items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null) item.Qty = Math.Max(1, qty);
        return items;
    }

    public static List<CartItem> Remove(List<CartItem> items, string productId)
    {
        items.RemoveAll(i => i.ProductId == productId);
        return items;
    }

    /// <summary>Frete: carrinho vazio=0; express=24,90; padrão=grátis ≥200, senão 14,90.</summary>
    public static CartTotals CalculateTotals(List<CartItem> items, bool express)
    {
        if (items is null || items.Count == 0) return new CartTotals(0m, 0m, 0m);
        var subtotal = items.Sum(i => i.Price * i.Qty);
        var frete = express ? 24.90m : subtotal >= 200m ? 0m : 14.90m;
        return new CartTotals(subtotal, frete, subtotal + frete);
    }
}

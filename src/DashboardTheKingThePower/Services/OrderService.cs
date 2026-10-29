using Blazored.LocalStorage;
using DashboardTheKingThePower.Models;

namespace DashboardTheKingThePower.Services;

public class OrderService
{
    private readonly StorageService<PlacedOrder> _store;

    public OrderService(ILocalStorageService localStorage)
        => _store = new StorageService<PlacedOrder>(localStorage, "dtkp:orders", Seed);

    public Task<List<PlacedOrder>> GetAsync() => _store.LoadAsync();
    public Task SaveAsync(List<PlacedOrder> orders) => _store.SaveAsync(orders);

    public static PlacedOrder Build(List<CartItem> items, string customer, bool express)
    {
        var totals = CartService.CalculateTotals(items, express);
        return new PlacedOrder
        {
            Id = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}",
            Customer = customer,
            Status = "Pendente",
            Subtotal = totals.Subtotal,
            Frete = totals.Frete,
            Total = totals.Total,
            Items = items.Select(i => new PlacedOrderItem
            {
                ProductId = i.ProductId,
                Name = i.Name,
                Price = i.Price,
                Qty = i.Qty
            }).ToList()
        };
    }

    // Seed: cópia exata da lista inline que vivia em Pages/Orders.razor (10 pedidos, dados inalterados).
    // OrderItem (Id/Customer/Date/Total/Status/Color) migrado para PlacedOrder: Subtotal = Total,
    // Frete = 0, CreatedAt = data do pedido; Color descartado (avatar agora deriva da iniciação).
    private static List<PlacedOrder> Seed() => new()
    {
        new() { Id = "ORD-001", Customer = "Alice Johnson",  Status = "Delivered", Subtotal = 289.99m,  Frete = 0m, Total = 289.99m,  CreatedAt = new DateTime(2026, 8, 1),  Items = DemoItems(289.99m) },
        new() { Id = "ORD-002", Customer = "Bob Williams",   Status = "Shipped",   Subtotal = 1549.50m, Frete = 0m, Total = 1549.50m, CreatedAt = new DateTime(2026, 8, 2),  Items = DemoItems(1549.50m) },
        new() { Id = "ORD-003", Customer = "Carol Davis",    Status = "Processing", Subtotal = 79.95m,  Frete = 0m, Total = 79.95m,   CreatedAt = new DateTime(2026, 8, 3),  Items = DemoItems(79.95m) },
        new() { Id = "ORD-004", Customer = "David Martinez", Status = "Pending",   Subtotal = 4299.00m, Frete = 0m, Total = 4299.00m, CreatedAt = new DateTime(2026, 8, 4),  Items = DemoItems(4299.00m) },
        new() { Id = "ORD-005", Customer = "Eva Anderson",   Status = "Delivered", Subtotal = 199.99m,  Frete = 0m, Total = 199.99m,  CreatedAt = new DateTime(2026, 8, 5),  Items = DemoItems(199.99m) },
        new() { Id = "ORD-006", Customer = "Frank Thomas",   Status = "Cancelled", Subtotal = 899.00m,  Frete = 0m, Total = 899.00m,  CreatedAt = new DateTime(2026, 8, 6),  Items = DemoItems(899.00m) },
        new() { Id = "ORD-007", Customer = "Grace Lee",      Status = "Shipped",   Subtotal = 349.50m,  Frete = 0m, Total = 349.50m,  CreatedAt = new DateTime(2026, 8, 7),  Items = DemoItems(349.50m) },
        new() { Id = "ORD-008", Customer = "Henry Wilson",   Status = "Processing", Subtotal = 1250.00m, Frete = 0m, Total = 1250.00m, CreatedAt = new DateTime(2026, 8, 8),  Items = DemoItems(1250.00m) },
        new() { Id = "ORD-009", Customer = "Ivy Chen",       Status = "Pending",   Subtotal = 67.50m,   Frete = 0m, Total = 67.50m,   CreatedAt = new DateTime(2026, 8, 9),  Items = DemoItems(67.50m) },
        new() { Id = "ORD-010", Customer = "Jack Brown",     Status = "Delivered", Subtotal = 2100.00m, Frete = 0m, Total = 2100.00m, CreatedAt = new DateTime(2026, 8, 10), Items = DemoItems(2100.00m) },
    };

    // O seed antigo não tinha itens reais: um item demo com Qty 1 e Price = Total (Subtotal = Price * Qty)
    private static List<PlacedOrderItem> DemoItems(decimal price) => new()
    {
        new() { Name = "Item do pedido", Price = price, Qty = 1 }
    };
}

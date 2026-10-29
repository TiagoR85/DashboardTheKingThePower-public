namespace DashboardTheKingThePower.Models;

public class PlacedOrder
{
    public string Id { get; set; } = "";
    public string Customer { get; set; } = "";
    public string Status { get; set; } = "Pendente"; // Pendente | Pago | Enviado | Entregue | Cancelado
    public decimal Subtotal { get; set; }
    public decimal Frete { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<PlacedOrderItem> Items { get; set; } = new();
}

public class PlacedOrderItem
{
    public string ProductId { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Qty { get; set; }
}

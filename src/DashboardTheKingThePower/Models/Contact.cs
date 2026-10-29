namespace DashboardTheKingThePower.Models;

public class Contact
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Company { get; set; } = "";
    public string Status { get; set; } = "Ativo"; // Ativo | Lead | Inativo
}

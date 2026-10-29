namespace DashboardTheKingThePower.Services;

/// <summary>Regra dos planos: anual = 10× mensal (2 meses grátis).</summary>
public static class PricingCalculator
{
    public static decimal AnnualTotal(decimal monthlyPrice) => monthlyPrice * 10m;
    public static decimal MonthlyEquivalent(decimal monthlyPrice) => AnnualTotal(monthlyPrice) / 12m;
    public static decimal AnnualSavings(decimal monthlyPrice) => monthlyPrice * 2m;
}

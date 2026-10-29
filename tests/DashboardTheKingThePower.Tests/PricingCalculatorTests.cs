using DashboardTheKingThePower.Services;

namespace DashboardTheKingThePower.Tests;

public class PricingCalculatorTests
{
    [Test]
    public void AnnualTotal_DezMesesParaDoze()
        => Assert.That(PricingCalculator.AnnualTotal(49.90m), Is.EqualTo(499.00m));

    [Test]
    public void AnnualSavings_DoisMesesGratis()
        => Assert.That(PricingCalculator.AnnualSavings(49.90m), Is.EqualTo(99.80m));

    [Test]
    public void MonthlyEquivalent_AnualDivididoPorDoze()
        => Assert.That(PricingCalculator.MonthlyEquivalent(49.90m), Is.EqualTo(499.00m / 12m));

    [Test]
    public void AnnualTotal_PrecoZero_Zerado()
        => Assert.That(PricingCalculator.AnnualTotal(0m), Is.EqualTo(0m));

    // Dívida: ramo de preço negativo — contrato linear documentado (sem clamp por design).
    [Test]
    public void AnnualTotal_PrecoNegativo_Proporcional()
        => Assert.That(PricingCalculator.AnnualTotal(-49.90m), Is.EqualTo(-499.00m));
}

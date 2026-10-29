using ApexCharts;
using DashboardTheKingThePower.Components.charts;

namespace DashboardTheKingThePower.Tests;

public class ChartDefaultsTests
{
    [Test]
    public void ApplyDefaults_ConfiguresAnimations()
    {
        var options = new ApexChartOptions<object>();

        ChartDefaults.ApplyDefaults(options);

        Assert.That(options.Chart!.Animations!.Enabled, Is.True);
        Assert.That(options.Chart.Animations.Speed, Is.EqualTo(400));
        Assert.That(options.Chart.Animations.AnimateGradually!.Enabled, Is.True);
        Assert.That(options.Chart.Animations.AnimateGradually.Delay, Is.EqualTo(150));
    }

    [Test]
    public void ApplyDefaults_ConfiguresGrid()
    {
        var options = new ApexChartOptions<object>();

        ChartDefaults.ApplyDefaults(options);

        Assert.That(options.Grid!.BorderColor, Is.EqualTo("#ffffff10"));
        Assert.That(options.Grid.StrokeDashArray, Is.EqualTo(4));
        Assert.That(options.Grid.Xaxis!.Lines!.Show, Is.False);
        Assert.That(options.Grid.Yaxis!.Lines!.Show, Is.True);
        Assert.That(options.Grid.Padding!.Top, Is.EqualTo(0));
        Assert.That(options.Grid.Padding.Right, Is.EqualTo(0));
        Assert.That(options.Grid.Padding.Bottom, Is.EqualTo(0));
        Assert.That(options.Grid.Padding.Left, Is.EqualTo(0));
    }

    [Test]
    public void ApplyDefaults_ConfiguresMarkersAndTooltip()
    {
        var options = new ApexChartOptions<object>();

        ChartDefaults.ApplyDefaults(options);

        Assert.That(options.Markers!.Hover!.SizeOffset, Is.EqualTo(3));
        Assert.That(options.Tooltip!.Style!.FontSize, Is.EqualTo("12px"));
        Assert.That(options.Tooltip.Style.FontFamily, Is.EqualTo("DM Sans, sans-serif"));
    }

    [Test]
    public void ApplyDefaults_IsIdempotent()
    {
        var options = new ApexChartOptions<object>();

        ChartDefaults.ApplyDefaults(options);
        ChartDefaults.ApplyDefaults(options);

        Assert.That(options.Chart!.Animations!.Speed, Is.EqualTo(400));
    }

    [Test]
    public void ApplyDefaults_PreservesExistingPartialConfiguration()
    {
        var options = new ApexChartOptions<object>
        {
            Chart = new Chart { Animations = new Animations { Speed = 123 } }
        };

        ChartDefaults.ApplyDefaults(options);

        Assert.That(options.Chart.Animations.Speed, Is.EqualTo(400));
        Assert.That(options.Chart.Animations.Enabled, Is.True);
    }
}

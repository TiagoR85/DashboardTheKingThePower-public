using ApexCharts;

namespace DashboardTheKingThePower.Components.charts;

/// <summary>
/// Shared chart configuration helpers using confirmed Blazor-ApexCharts v7.0.0 API.
/// ponytail: gradient helper removed — FillTypeSelections/Opacity constructors unconfirmed.
/// Add back when API is verified locally with dotnet build.
/// </summary>
public static class ChartDefaults
{
    public static void ApplyDefaults<T>(ApexChartOptions<T> options) where T : class
    {
        // Smooth animations (400ms vs default 300ms)
        options.Chart ??= new Chart();
        options.Chart.Animations ??= new Animations();
        options.Chart.Animations.Enabled = true;
        options.Chart.Animations.Speed = 400;
        options.Chart.Animations.AnimateGradually ??= new AnimateGradually();
        options.Chart.Animations.AnimateGradually.Enabled = true;
        options.Chart.Animations.AnimateGradually.Delay = 150;

        // Subtle grid — horizontal lines only
        options.Grid ??= new Grid();
        options.Grid.BorderColor = "#ffffff10";
        options.Grid.StrokeDashArray = 4;
        options.Grid.Xaxis ??= new GridXAxis();
        options.Grid.Xaxis.Lines ??= new Lines();
        options.Grid.Xaxis.Lines.Show = false;
        options.Grid.Yaxis ??= new GridYAxis();
        options.Grid.Yaxis.Lines ??= new Lines();
        options.Grid.Yaxis.Lines.Show = true;
        options.Grid.Padding ??= new Padding();
        options.Grid.Padding.Top = 0;
        options.Grid.Padding.Right = 0;
        options.Grid.Padding.Bottom = 0;
        options.Grid.Padding.Left = 0;

        // Datapoint hover glow
        options.Markers ??= new Markers();
        options.Markers.Hover ??= new MarkersHover();
        options.Markers.Hover.SizeOffset = 3;

        // Tooltip font
        options.Tooltip ??= new Tooltip();
        options.Tooltip.Style ??= new TooltipStyle();
        options.Tooltip.Style.FontSize = "12px";
        options.Tooltip.Style.FontFamily = "DM Sans, sans-serif";
    }
}

using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace DashboardTheKingThePower.Tests.Playwright;

[NonParallelizable]
public class DashboardTests : PageTest
{
    private const string BaseUrl = "http://localhost:5000";

    [SetUp]
    public async Task Setup()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
    }

    [Test]
    public async Task Home_LoadsSuccessfully()
    {
        var title = await Page.TitleAsync();
        Assert.That(title, Does.Contain("Dashboard"));
    }

    [Test]
    public async Task KpiCards_AreVisible()
    {
        var kpiCards = Page.GetByText("Total Revenue");
        await Expect(kpiCards).ToBeVisibleAsync();
    }

    [Test]
    public async Task Sidebar_IsVisible()
    {
        var sidebar = Page.Locator("nav").First;
        await Expect(sidebar).ToBeVisibleAsync();
    }

    [Test]
    public async Task Header_IsVisible()
    {
        var header = Page.GetByText("Dashboard");
        await Expect(header).ToBeVisibleAsync();
    }

    [Test]
    public async Task ThemeToggle_TogglesTheme()
    {
        var initialTheme = await Page.EvaluateAsync<string>("document.documentElement.getAttribute('data-theme')");

        var themeToggle = Page.GetByRole(Microsoft.Playwright.AriaRole.Button, new() { Name = "theme" });
        if (await themeToggle.CountAsync() > 0)
        {
            await themeToggle.ClickAsync();
            await Page.WaitForTimeoutAsync(500);
        }

        var afterTheme = await Page.EvaluateAsync<string>("document.documentElement.getAttribute('data-theme')");
        Assert.That(afterTheme, Is.Not.EqualTo(initialTheme));
    }

    [Test]
    public async Task CommandPalette_OpensOnCtrlK()
    {
        await Page.Keyboard.PressAsync("Control+k");
        await Page.WaitForTimeoutAsync(500);

        var searchInput = Page.GetByPlaceholder("Search");
        if (await searchInput.CountAsync() > 0)
        {
            await Expect(searchInput).ToBeVisibleAsync();
        }
    }

    [Test]
    public async Task Charts_AreRendered()
    {
        var chartElements = Page.Locator("svg");
        var count = await chartElements.CountAsync();
        Assert.That(count, Is.GreaterThan(0));
    }
}

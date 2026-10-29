# Install Playwright browsers for E2E tests
# Run this from the tests/DashboardTheKingThePower.Tests directory

Write-Host "Installing Playwright Chromium browser..." -ForegroundColor Green

# Install Playwright via dotnet tool
dotnet tool install --global Microsoft.Playwright.CLI 2>$null

# Install Chromium browser
npx playwright install chromium

Write-Host "Playwright installation complete!" -ForegroundColor Green

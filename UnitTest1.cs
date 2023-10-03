using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightTests
{
    [TestClass]
    public class UnitTest1 : PageTest
    {
        private IBrowser? browser;
        private IPlaywright? playwright;
        private IPage? page;

        [TestMethod]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            // Expect a title "to contain" a substring.
            await Expect(page).ToHaveTitleAsync(new Regex("Playwright"));

            // create a locator
            var getStarted = page.GetByRole(AriaRole.Link, new() { Name = "Get started" });

            // Expect an attribute "to be strictly equal" to the value.
            await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

            // Click the get started link.
            await getStarted.ClickAsync();

            // Expects the URL to contain intro.
            await Expect(page).ToHaveURLAsync(new Regex(".*intro"));

            await GetScreenShot(page);
        }
        [TestInitialize]
        public async Task Initialize()
        {
            playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new()
            {
                Headless = false,
                SlowMo = 100,
            });
            page = await browser.NewPageAsync();
            await page.GotoAsync("https://playwright.dev");
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            await browser.CloseAsync();
            await browser.DisposeAsync();
        }

        private async Task GetScreenShot(IPage _page)
        {
            string screenshotPath = GetScreenshotPath();
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
        }

        private string GetScreenshotPath()
        {
            string path = Directory.GetCurrentDirectory();
            int lastIndex = path.LastIndexOf("bin");
            var screenshotDirectory = path.Substring(0, lastIndex) + "TestResults\\Screenshots\\";
            Directory.CreateDirectory(screenshotDirectory);
            var timestamp = DateTime.Now.ToString("ddMMyyyyHHmmssfff");
            var screenshotFileName = $"screenshot_{timestamp}.png";
            var screenshotPath = Path.Combine(screenshotDirectory, screenshotFileName);
            return screenshotPath.ToString();
        }
    }
}
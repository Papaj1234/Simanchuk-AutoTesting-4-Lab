using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace NUnitProject;

[Collection("Sequential")]
public class EhuUniversityTests : IDisposable
{
    private readonly IWebDriver driver;
    private const string BaseUrl = "https://en.ehuniversity.lt/";

    public EhuUniversityTests()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        driver = new ChromeDriver(options);
    }

    public void Dispose()
    {
        driver.Quit();
        driver.Dispose();
    }

    private WebDriverWait Wait => new WebDriverWait(driver, TimeSpan.FromSeconds(10));

    public static IEnumerable<object[]> SearchTerms =>
        new List<object[]>
        {
            new object[] { "study programs" },
            new object[] { "admission" }
        };

    [Fact]
    [Trait("Category", "Navigation")]
    public void NavigateToAboutPage_ShouldOpenCorrectUrl()
    {
        driver.Navigate().GoToUrl(BaseUrl);
        Wait.Until(d => d.FindElement(By.ClassName("toggle-header-menu"))).Click();
        Wait.Until(d =>
            d.FindElement(By.XPath("//ul[contains(@class,'menu')]//a[contains(text(),'About')]")))
           .Click();
        Assert.Equal("https://en.ehuniversity.lt/about/", driver.Url);
    }

    [Theory]
    [MemberData(nameof(SearchTerms))]
    [Trait("Category", "Search")]
    public void SearchByTerm_ShouldRedirectWithQueryInUrl(string searchTerm)
    {
        driver.Navigate().GoToUrl(BaseUrl);
        var searchTrigger = driver.FindElement(By.ClassName("header-search"));
        new Actions(driver).MoveToElement(searchTrigger).Perform();
        Wait.Until(d =>
            d.FindElement(By.CssSelector(".header-search__form input[name='s']")))
           .SendKeys(searchTerm + Keys.Enter);
        Assert.Contains(searchTerm.Replace(" ", "+"), driver.Url);
    }

    [Fact]
    [Trait("Category", "Localization")]
    public void SwitchLanguageToLithuanian_ShouldChangeUrl()
    {
        driver.Navigate().GoToUrl(BaseUrl);
        var langSwitcher = Wait.Until(d =>
            d.FindElement(By.CssSelector("ul.language-switcher")));
        new Actions(driver).MoveToElement(langSwitcher).Perform();
        Wait.Until(d =>
            d.FindElement(By.XPath("//ul[contains(@class,'language-switcher')]//a[contains(@href,'lt.ehuniversity.lt')]")))
           .Click();
        Assert.Contains("lt.ehuniversity.lt", driver.Url);
    }

    [Fact]
    [Trait("Category", "Content")]
    public void ContactPage_ShouldContainExpectedEmail()
    {
        driver.Navigate().GoToUrl("https://en.ehuniversity.lt/research/projects/contact-us/");
        var contactList = Wait.Until(d =>
            d.FindElement(By.CssSelector("ul.wp-block-list")));
        Assert.Contains("franciskscarynacr@gmail.com", contactList.Text);
    }
}
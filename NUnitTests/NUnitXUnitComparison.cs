using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace NUnitXUnitComparison;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public class EhuUniversityTests
{
    private ThreadLocal<IWebDriver> driverHolder = new ThreadLocal<IWebDriver>(true);
    private const string BaseUrl = "https://en.ehuniversity.lt/";

    private IWebDriver Driver => driverHolder.Value!;

    private WebDriverWait CreateWait(int seconds = 10) =>
        new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        driverHolder.Value = new ChromeDriver(options);
    }

    [TearDown]
    public void Teardown()
    {
        Driver.Quit();
        driverHolder.Value!.Dispose();
    }

    private static IEnumerable<string> SearchTerms() => new[] { "study programs", "admission" };

    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        driverHolder.Dispose();
    }

    [Test]
    [Category("Navigation")]
    public void NavigateToAboutPage_ShouldOpenCorrectUrl()
    {
        var wait = CreateWait();
        Driver.Navigate().GoToUrl(BaseUrl);

        wait.Until(d => d.FindElement(By.ClassName("toggle-header-menu"))).Click();

        wait.Until(d =>
            d.FindElement(By.XPath("//ul[contains(@class,'menu')]//a[contains(text(),'About')]")))
           .Click();

        Assert.Multiple(() =>
        {
            Assert.That(Driver.Url, Is.EqualTo("https://en.ehuniversity.lt/about/"));
            Assert.That(Driver.FindElement(By.TagName("h1")).Text, Is.EqualTo("About"));
        });
    }

    [Test]
    [Category("Search")]
    [TestCaseSource(nameof(SearchTerms))]
    public void SearchByTerm_ShouldRedirectWithQueryInUrl(string searchTerm)
    {
        var wait = CreateWait();
        Driver.Navigate().GoToUrl(BaseUrl);

        var searchTrigger = Driver.FindElement(By.ClassName("header-search"));
        new Actions(Driver).MoveToElement(searchTrigger).Perform();

        wait.Until(d =>
            d.FindElement(By.CssSelector(".header-search__form input[name='s']")))
           .SendKeys(searchTerm + Keys.Enter);

        Assert.That(Driver.Url, Does.Contain(searchTerm.Replace(" ", "+")));
    }

    [Test]
    [Category("Localization")]
    public void SwitchLanguageToLithuanian_ShouldChangeUrl()
    {
        var wait = CreateWait();
        Driver.Navigate().GoToUrl(BaseUrl);

        var langSwitcher = wait.Until(d =>
            d.FindElement(By.CssSelector("ul.language-switcher")));
        new Actions(Driver).MoveToElement(langSwitcher).Perform();

        wait.Until(d =>
            d.FindElement(By.XPath("//ul[contains(@class,'language-switcher')]//a[contains(@href,'lt.ehuniversity.lt')]")))
           .Click();

        Assert.That(Driver.Url, Does.Contain("lt.ehuniversity.lt"));
    }

    [Test]
    [Category("Content")]
    public void ContactPage_ShouldContainExpectedEmail()
    {
        var wait = CreateWait();
        Driver.Navigate().GoToUrl("https://en.ehuniversity.lt/research/projects/contact-us/");

        var contactList = wait.Until(d =>
            d.FindElement(By.CssSelector("ul.wp-block-list")));

        Assert.That(contactList.Text, Does.Contain("franciskscarynacr@gmail.com"));
    }
}
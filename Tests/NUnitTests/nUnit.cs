using NUnit.Framework;
using SeleniumPatterns.Builder;
using SeleniumPatterns.Core;
using SeleniumPatterns.Factory;

namespace SeleniumPatterns.Tests.NUnitTests;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public class EhuUniversityNUnitTests
{
    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptionsBuilder().WithMaximizedWindow().Build();

        WebDriverManager.Instance.Initialize(options);
    }

    [TearDown]
    public void Teardown() => WebDriverManager.Instance.Quit();

    [Test]
    [Category("Navigation")]
    public void NavigateToAboutPage_ShouldOpenCorrectUrl()
    {
        var aboutPage = PageFactory.CreateHomePage().Open().NavigateToAbout();

        Assert.Multiple(() =>
        {
            Assert.That(aboutPage.CurrentUrl, Is.EqualTo("https://en.ehuniversity.lt/about/"));
            Assert.That(aboutPage.GetHeadingText(), Is.EqualTo("About"));
        });
    }

    private static IEnumerable<string> SearchTerms() => new[] { "study programs", "admission" };

    [Test]
    [Category("Search")]
    [TestCaseSource(nameof(SearchTerms))]
    public void SearchByTerm_ShouldRedirectWithQueryInUrl(string searchTerm)
    {
        var results = PageFactory.CreateHomePage().Open().Search(searchTerm);
        Assert.That(results.ResultUrl, Does.Contain(searchTerm.Replace(" ", "+")));
    }

    [Test]
    [Category("Localization")]
    public void SwitchLanguageToLithuanian_ShouldChangeUrl()
    {
        var home = PageFactory.CreateHomePage().Open();
        home.SwitchToLithuanian();
        Assert.That(home.CurrentUrl, Does.Contain("lt.ehuniversity.lt"));
    }

    [Test]
    [Category("Content")]
    public void ContactPage_ShouldContainExpectedEmail()
    {
        var contact = PageFactory.CreateContactPage().Open();
        Assert.That(contact.GetContactListText(), Does.Contain("franciskscarynacr@gmail.com"));
    }
}
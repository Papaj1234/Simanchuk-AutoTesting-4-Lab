using SeleniumPatterns.Builder;
using SeleniumPatterns.Core;
using SeleniumPatterns.Factory;
using Xunit;

namespace SeleniumPatterns.Tests.XUnitTests;

[Collection("Sequential")]
public class EhuUniversityXUnitTests : IDisposable
{
    public EhuUniversityXUnitTests()
    {
        var options = new ChromeOptionsBuilder().WithMaximizedWindow().Build();

        WebDriverManager.Instance.Initialize(options);
    }

    public void Dispose() => WebDriverManager.Instance.Quit();

    [Fact]
    [Trait("Category", "Navigation")]
    public void NavigateToAboutPage_ShouldOpenCorrectUrl()
    {
        var aboutPage = PageFactory.CreateHomePage().Open().NavigateToAbout();

        Assert.Equal("https://en.ehuniversity.lt/about/", aboutPage.CurrentUrl);
        Assert.Equal("About", aboutPage.GetHeadingText());
    }

    public static IEnumerable<object[]> SearchTerms =>
        new List<object[]>
        {
            new object[] { "study programs" },
            new object[] { "admission" }
        };

    [Theory]
    [MemberData(nameof(SearchTerms))]
    [Trait("Category", "Search")]
    public void SearchByTerm_ShouldRedirectWithQueryInUrl(string searchTerm)
    {
        var results = PageFactory.CreateHomePage().Open().Search(searchTerm);
        Assert.Contains(searchTerm.Replace(" ", "+"), results.ResultUrl);
    }

    [Fact]
    [Trait("Category", "Localization")]
    public void SwitchLanguageToLithuanian_ShouldChangeUrl()
    {
        var home = PageFactory.CreateHomePage().Open();
        home.SwitchToLithuanian();
        Assert.Contains("lt.ehuniversity.lt", home.CurrentUrl);
    }

    [Fact]
    [Trait("Category", "Content")]
    public void ContactPage_ShouldContainExpectedEmail()
    {
        var contact = PageFactory.CreateContactPage().Open();
        Assert.Contains("franciskscarynacr@gmail.com", contact.GetContactListText());
    }
}

[CollectionDefinition("Sequential", DisableParallelization = true)]
public class SequentialCollection { }
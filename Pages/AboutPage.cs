using OpenQA.Selenium;

namespace SeleniumPatterns.Pages;

public class AboutPage : BasePage
{
    public const string ExpectedUrl = "https://en.ehuniversity.lt/about/";

    private static readonly By PageHeading = By.TagName("h1");

    public string GetHeadingText() => WaitForElement(PageHeading).Text;
}
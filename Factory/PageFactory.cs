using SeleniumPatterns.Pages;

namespace SeleniumPatterns.Factory;

public static class PageFactory
{
    public static HomePage CreateHomePage() => new HomePage();
    public static AboutPage CreateAboutPage() => new AboutPage();
    public static ContactPage CreateContactPage() => new ContactPage();
    public static SearchResultsPage CreateSearchResultsPage() => new SearchResultsPage();
}
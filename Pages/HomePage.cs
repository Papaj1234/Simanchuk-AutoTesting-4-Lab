using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SeleniumPatterns.Pages;

public class HomePage : BasePage
{
    public const string Url = "https://en.ehuniversity.lt/";

    private static readonly By MenuToggle = By.ClassName("toggle-header-menu");
    private static readonly By AboutMenuLink = By.XPath("//ul[contains(@class,'menu')]//a[contains(text(),'About')]");
    private static readonly By SearchTrigger = By.ClassName("header-search");
    private static readonly By SearchInput = By.CssSelector(".header-search__form input[name='s']");
    private static readonly By LanguageSwitcher = By.CssSelector("ul.language-switcher");
    private static readonly By LithuanianLangLink = By.XPath("//ul[contains(@class,'language-switcher')]//a[contains(@href,'lt.ehuniversity.lt')]");

    public HomePage Open()
    {
        Driver.Navigate().GoToUrl(Url);
        return this;
    }

    public AboutPage NavigateToAbout()
    {
        ClickWhen(MenuToggle);
        ClickWhen(AboutMenuLink);
        return new AboutPage();
    }

    public SearchResultsPage Search(string term)
    {
        var trigger = WaitForElement(SearchTrigger);
        new Actions(Driver).MoveToElement(trigger).Perform();
        WaitForElement(SearchInput).SendKeys(term + Keys.Enter);
        return new SearchResultsPage();
    }

    public void SwitchToLithuanian()
    {
        var switcher = WaitForElement(LanguageSwitcher);
        new Actions(Driver).MoveToElement(switcher).Perform();
        ClickWhen(LithuanianLangLink);
    }
}
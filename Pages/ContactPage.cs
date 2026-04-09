using OpenQA.Selenium;

namespace SeleniumPatterns.Pages;

public class ContactPage : BasePage
{
    public const string Url = "https://en.ehuniversity.lt/research/projects/contact-us/";

    private static readonly By ContactList = By.CssSelector("ul.wp-block-list");

    public ContactPage Open()
    {
        Driver.Navigate().GoToUrl(Url);
        return this;
    }

    public string GetContactListText() => WaitForElement(ContactList).Text;
}
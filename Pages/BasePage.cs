using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPatterns.Core;

namespace SeleniumPatterns.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage()
    {
        Driver = WebDriverManager.Instance.Driver;
        Wait = WebDriverManager.Instance.CreateWait();
    }

    protected IWebElement WaitForElement(By locator) => Wait.Until(d => d.FindElement(locator));

    protected void ClickWhen(By locator) => WaitForElement(locator).Click();

    public string CurrentUrl => Driver.Url;
}
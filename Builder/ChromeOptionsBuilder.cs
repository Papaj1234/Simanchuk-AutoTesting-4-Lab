using OpenQA.Selenium.Chrome;

namespace SeleniumPatterns.Builders;

public class ChromeOptionsBuilder
{
    private readonly ChromeOptions options = new ChromeOptions();

    public ChromeOptionsBuilder WithMaximizedWindow()
    {
        options.AddArgument("--start-maximized");
        return this;
    }

    public ChromeOptionsBuilder WithHeadless()
    {
        options.AddArgument("--headless=new");
        options.AddArgument("--window-size=1920,1080");
        return this;
    }

    public ChromeOptionsBuilder WithNoSandbox()
    {
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        return this;
    }

    public ChromeOptionsBuilder WithArgument(string argument)
    {
        options.AddArgument(argument);
        return this;
    }

    public ChromeOptions Build() => options;
}
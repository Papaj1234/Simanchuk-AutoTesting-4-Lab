using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPatterns.Core;

public sealed class WebDriverManager
{
	private static readonly ThreadLocal<WebDriverManager> instance = new ThreadLocal<WebDriverManager>(() => new WebDriverManager());

	private IWebDriver? driver;

	private WebDriverManager() { }

	public static WebDriverManager Instance => instance.Value!;

	public IWebDriver Driver => driver ?? throw new InvalidOperationException ("WebDriver is not initialized. Call Initialize() before use.");

	public void Initialize(ChromeOptions options)
	{
		driver = new ChromeDriver(options);
	}

	public WebDriverWait CreateWait(int seconds = 10) => new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));

	public void Quit()
	{
		driver?.Quit();
		driver?.Dispose();
		driver = null;
	}
}
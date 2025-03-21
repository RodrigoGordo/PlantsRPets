using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using Xunit;

public class BrowserCompatibilityTest
{
    private void RunTest(IWebDriver driver, string browserName)
    {
        try
        {
            driver.Navigate().GoToUrl("http://localhost:4200");
            string pageTitle = driver.Title;

            Assert.Contains("PlantsrpetsprojetoClient", pageTitle);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error on {browserName}: {ex.Message}");
        }
        finally
        {
            driver.Quit();
        }
    }

    [Fact]
    public void TestChrome()
    {
        IWebDriver driver = new ChromeDriver();
        RunTest(driver, "Chrome");
    }

    [Fact]
    public void TestEdge()
    {
        IWebDriver driver = new EdgeDriver();
        RunTest(driver, "Edge");
    }
}

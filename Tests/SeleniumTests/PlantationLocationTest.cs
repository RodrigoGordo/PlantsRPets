using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using OpenQA.Selenium.Edge;

public class PlantationLocationTest : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private const string BaseUrl = "https://localhost:4200";
    private const string TestEmail = "plantsrpets@outlook.com";
    private const string TestPassword = "PrP#2025";

    public PlantationLocationTest()
    {
        _driver = new EdgeDriver();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        _driver.Manage().Window.Maximize();
    }

    [Fact]
    public void CreatePlantation_WithLocation_ShowsCorrectLocation()
    {
        // Login first
        _driver.Navigate().GoToUrl($"{BaseUrl}");

        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        var loginMenu = wait.Until(driver => driver.FindElement(By.ClassName("login-menu")));
        loginMenu.Click();

        var emailField = _driver.FindElement(By.CssSelector("input[placeholder='Email address']"));
        var passwordField = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));

        emailField.SendKeys(TestEmail);
        passwordField.SendKeys(TestPassword);

        var continueButton = wait.Until(driver =>
        {
            var button = driver.FindElement(By.CssSelector("button.signin-btn[type='submit']"));
            return button.Enabled ? button : null;
        });

        continueButton.Click();

        // Navigate to plantations page
        _wait.Until(d => d.FindElement(By.ClassName("header")));
        _driver.Navigate().GoToUrl($"{BaseUrl}/plantations");

        // Open creation dialog
        var createButton = _wait.Until(d => d.FindElement(By.ClassName("create-btn")));
        createButton.Click();

        // Fill plantation details
        var nameInput = _wait.Until(d => d.FindElement(By.CssSelector("input#name")));
        nameInput.SendKeys("Automation Test Plantation");

        // Select plant type (first option)
        var typeSelect = _driver.FindElement(By.ClassName("option"));
        new SelectElement(typeSelect).SelectByIndex(1);

        // Handle location search
        var locationInput = _wait.Until(d =>
            d.FindElement(By.CssSelector("app-location-input input")));
        locationInput.SendKeys("Lisboa");

        // Select first search result
        var firstResult = _wait.Until(d =>
            d.FindElement(By.CssSelector(".search-result-item")));
        firstResult.Click();

        // Submit form
        _driver.FindElement(By.CssSelector("button.confirm-btn")).Click();

        // Verify created plantation
        var locationElements = _wait.Until(d =>
            d.FindElements(By.CssSelector(".plantation-card .location p")));

        Assert.Contains("Lisboa, Lisboa, Portugal",
            locationElements[^1].Text); // Check last added card
    }

    public void Dispose()
    {
        _driver.Quit();
    }
}
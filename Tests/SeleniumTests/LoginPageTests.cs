using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using System;

namespace Tests.SeleniumTests
{
    public class LoginPageTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string baseUrl = "http://localhost:4200";


        public LoginPageTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            _driver = new ChromeDriver(options);
        }

        [Fact]
        public void TestLogin()
        {
            _driver.Navigate().GoToUrl($"{baseUrl}");

            var loginMenu = _driver.FindElement(By.Id("login-menu"));
            loginMenu.Click();

            var emailField = _driver.FindElement(By.CssSelector("input[placeholder='Email address']"));
            var passwordField = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            var continueButton = _driver.FindElement(By.XPath("//button[contains(text(), 'Continue')]"));

            emailField.SendKeys("gega@gmail.com");
            passwordField.SendKeys("gegassaurorex#");
            continueButton.Click();


            // Se encontrar o elemento collections, então fez login com sucesso e está na home page 
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            var collectionsElement = wait.Until(driver =>
                driver.FindElement(By.XPath("//a[@routerlink='/collections' and contains(text(), 'Collections')]")));

            Assert.True(collectionsElement.Displayed, "O componente 'Collections' foi encontrado, indicando que o login foi bem-sucedido.");
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}

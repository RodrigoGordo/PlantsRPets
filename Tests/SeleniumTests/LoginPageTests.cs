using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using System;

namespace Tests.SeleniumTests
{
    public class LoginPageTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string baseUrl = "https://localhost:4200";


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

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var loginMenu = wait.Until(driver => driver.FindElement(By.ClassName("login-menu")));
            loginMenu.Click();

            var emailField = _driver.FindElement(By.CssSelector("input[placeholder='Email address']"));
            var passwordField = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));

            emailField.SendKeys("gega@gmail.com");
            passwordField.SendKeys("gegassaurorex#");


            var continueButton = wait.Until(driver =>
            {
                var button = driver.FindElement(By.CssSelector("button.signin-btn[type='submit']"));
                return button.Enabled ? button : null;
            });

            continueButton.Click();


            // Se encontrar o elemento collections, então fez login com sucesso e está na home page 
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

using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using System;
using System.Threading;

namespace Tests.SeleniumTests
{
    public class WeatherServiceSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string baseUrl = "https://localhost:4200";

        public WeatherServiceSeleniumTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            // Configurar para permitir a geolocalização
            options.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 1);


            _driver = new ChromeDriver(options);
        }

        //[Fact]
        //public void TestLoginAndCheckWeatherForecastWithLocation()
        //{
        //    // Ativar geolocalização com coordenadas para Cologne, Alemanha
        //    var latitude = "50.9575";
        //    var longitude = "6.8025";

        //    ((IJavaScriptExecutor)_driver).ExecuteScript(
        //        $"navigator.geolocation.getCurrentPosition = function(success) {{ success({{ coords: {{ latitude: {latitude}, longitude: {longitude} }} }}); }}");

        //    _driver.Navigate().GoToUrl($"{baseUrl}");

        //    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        //    var loginMenu = wait.Until(driver => driver.FindElement(By.ClassName("login-menu")));
        //    loginMenu.Click();

        //    var emailField = _driver.FindElement(By.CssSelector("input[placeholder='Email address']"));
        //    var passwordField = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));

        //    emailField.SendKeys("gega@gmail.com");
        //    passwordField.SendKeys("gegassaurorex#");

        //    var continueButton = wait.Until(driver =>
        //    {
        //        var button = driver.FindElement(By.CssSelector("button.signin-btn[type='submit']"));
        //        return button.Enabled ? button : null;
        //    });

        //    continueButton.Click();

        //    var collectionsElement = wait.Until(driver =>
        //                    driver.FindElement(By.XPath("//a[@routerlink='/collections' and contains(text(), 'Collections')]")));

        //    Assert.True(collectionsElement.Displayed, "O componente 'Collections' foi encontrado, indicando que o login foi bem-sucedido.");

        //    var weatherForecastElement = wait.Until(driver =>
        //        driver.FindElement(By.XPath("//div[contains(@class, 'weather-container')]")));

        //    Assert.True(weatherForecastElement.Displayed, "O componente 'Weather Forecast' foi encontrado na página inicial.");

        //    // Verificar se a cidade é a correta (Cologne, Alemanha)
        //    var cityElement = wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class, 'weather-info')]//h3")));
        //    var cityName = cityElement.Text;

        //    Assert.Equal("Cologne", cityName);

        //}

        [Fact]
        public void TestLoginAndCheckWeatherForecastWithoutLocation()
        {
            // Não ativar a geolocalização (vai usar a localização default)
            _driver.Navigate().GoToUrl($"{baseUrl}");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
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

            var collectionsElement = wait.Until(driver =>
                            driver.FindElement(By.XPath("//a[@routerlink='/collections' and contains(text(), 'Collections')]")));

            Assert.True(collectionsElement.Displayed, "O componente 'Collections' foi encontrado, indicando que o login foi bem-sucedido.");

            var weatherForecastElement = wait.Until(driver =>
                driver.FindElement(By.XPath("//div[contains(@class, 'weather-container')]")));

            Assert.True(weatherForecastElement.Displayed, "O componente 'Weather Forecast' foi encontrado na página inicial.");

            // Verificar se encontra a cidade padrão (Setúbal)
            var cityElement = _driver.FindElement(By.XPath("//div[contains(@class, 'weather-info')]//h3"));
            var cityName = cityElement.Text;

            Assert.Equal("Setubal", cityName);

        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}

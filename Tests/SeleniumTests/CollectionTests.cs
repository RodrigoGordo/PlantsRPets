using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlantsRPetsProjeto.Tests.SeleniumTests
{
    public class CollectionTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string baseUrl = "https://localhost:4200";
        private readonly WebDriverWait _wait;

        public CollectionTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private void Login(string email, string password)
        {
            _driver.Navigate().GoToUrl($"{baseUrl}");

            var loginMenu = _wait.Until(driver => driver.FindElement(By.ClassName("login-menu")));
            loginMenu.Click();

            var emailField = _driver.FindElement(By.CssSelector("input[placeholder='Email address']"));
            var passwordField = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));

            emailField.SendKeys(email);
            passwordField.SendKeys(password);

            var continueButton = _wait.Until(driver =>
            {
                var button = driver.FindElement(By.CssSelector("button.signin-btn[type='submit']"));
                return button.Enabled ? button : null;
            });

            continueButton.Click();

            _wait.Until(driver =>
                driver.FindElement(By.XPath("//a[@routerlink='/collections' and contains(text(), 'Collections')]")));
        }

        private void NavigateToCollections()
        {
            var collectionsLink = _driver.FindElement(By.XPath("//a[@routerlink='/collections']"));
            collectionsLink.Click();

            _wait.Until(driver => driver.FindElement(By.ClassName("collection-container")));
        }

        [Fact]
        public void AdminUser_ShouldSeeAllPetsUnlocked()
        {
            Login("plantsrpets@outlook.com", "PrP#2025");
            NavigateToCollections();

            _wait.Until(driver =>
                !driver.FindElements(By.ClassName("loading")).Any(e => e.Displayed));

            var petCards = _driver.FindElements(By.ClassName("pet-card"));

            var notOwnedPets = petCards.Where(card => card.GetAttribute("class").Contains("not-owned")).ToList();

            Assert.Empty(notOwnedPets);
            Assert.True(petCards.Count > 0, "Admin should have pets in collection");

            foreach (var petCard in petCards)
            {
                var petName = petCard.FindElement(By.TagName("h3")).Text;
                Assert.NotEqual("???", petName);

                Assert.True(petCard.FindElements(By.ClassName("favorite-button")).Count > 0);
            }
        }

        [Fact]
        public void NormalUser_ShouldSeeNoPetsUnlocked()
        {
            Login("gega@gmail.com", "gegassaurorex#");
            NavigateToCollections();

            _wait.Until(driver =>
                !driver.FindElements(By.ClassName("loading")).Any(e => e.Displayed));

            var petCards = _driver.FindElements(By.ClassName("pet-card"));

            var ownedPets = petCards.Where(card => !card.GetAttribute("class").Contains("not-owned")).ToList();

            Assert.Empty(ownedPets);
            Assert.True(petCards.Count > 0, "User should see pet silhouettes even if not owned");

            foreach (var petCard in petCards)
            {
                var petName = petCard.FindElement(By.TagName("h3")).Text;
                Assert.Equal("???", petName);

                Assert.True(petCard.FindElements(By.ClassName("favorite-button")).Count == 0);
            }
        }

        [Fact]
        public void AdminUser_CanToggleFavoriteStatus()
        {
            Login("plantsrpets@outlook.com", "PrP#2025");
            NavigateToCollections();

            _wait.Until(driver =>
                !driver.FindElements(By.ClassName("loading")).Any(e => e.Displayed));

            var firstPetCard = _driver.FindElements(By.ClassName("pet-card"))
                .FirstOrDefault(card => !card.GetAttribute("class").Contains("not-owned"));

            Assert.NotNull(firstPetCard);

            var favoriteButton = firstPetCard.FindElement(By.ClassName("favorite-button"));

            bool initialIsFavorite = favoriteButton.GetAttribute("class").Contains("is-favorite");

            favoriteButton.Click();

            _wait.Until(driver => {
                var updatedButton = firstPetCard.FindElement(By.ClassName("favorite-button"));
                bool currentIsFavorite = updatedButton.GetAttribute("class").Contains("is-favorite");
                return currentIsFavorite != initialIsFavorite;
            });

            var updatedFavoriteButton = firstPetCard.FindElement(By.ClassName("favorite-button"));
            bool newIsFavorite = updatedFavoriteButton.GetAttribute("class").Contains("is-favorite");

            Assert.NotEqual(initialIsFavorite, newIsFavorite);
        }

        [Fact]
        public void AdminUser_CanViewPetDetails()
        {
            Login("plantsrpets@outlook.com", "PrP#2025");
            NavigateToCollections();

            _wait.Until(driver =>
                !driver.FindElements(By.ClassName("loading")).Any(e => e.Displayed));

            var firstOwnedPet = _driver.FindElements(By.ClassName("pet-card"))
                .FirstOrDefault(card => !card.GetAttribute("class").Contains("not-owned"));

            Assert.NotNull(firstOwnedPet);

            var petName = firstOwnedPet.FindElement(By.TagName("h3")).Text;

            firstOwnedPet.Click();

            _wait.Until(driver => driver.Url.Contains("/pet/"));

        }

        [Fact]
        public void NormalUser_CannotViewPetDetails()
        {
            Login("gega@gmail.com", "gegassaurorex#");
            NavigateToCollections();

            _wait.Until(driver =>
                !driver.FindElements(By.ClassName("loading")).Any(e => e.Displayed));

            string collectionPageUrl = _driver.Url;

            var firstPetCard = _driver.FindElements(By.ClassName("pet-card")).First();

            Assert.Contains("not-owned", firstPetCard.GetAttribute("class"));

            firstPetCard.Click();

            System.Threading.Thread.Sleep(1000);

            Assert.Equal(collectionPageUrl, _driver.Url);
        }

        [Fact]
        public void TestCollectionPageLoading()
        {
            Login("plantsrpets@outlook.com", "PrP#2025");
            NavigateToCollections();

            var loadingIndicators = _driver.FindElements(By.ClassName("loading"));
            if (loadingIndicators.Any(e => e.Displayed))
            {
                _wait.Until(driver =>
                    !driver.FindElements(By.ClassName("loading")).Any(e => e.Displayed));
            }

            var petContainer = _driver.FindElement(By.ClassName("pet-container"));
            Assert.True(petContainer.Displayed);

            var errorMessages = _driver.FindElements(By.ClassName("error-message"));
            Assert.False(errorMessages.Any(e => e.Displayed));
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
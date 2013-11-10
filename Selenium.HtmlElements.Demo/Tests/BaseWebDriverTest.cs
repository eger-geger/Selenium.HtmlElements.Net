using System;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using Selenium.HtmlElements.Factory;

namespace Selenium.HtmlElements.Demo.Tests {

    [TestFixture]
    public class BaseWebDriverTest {

        private IWebDriver _webDriver;

        private void InitWebDriver() {
            _webDriver = new ChromeDriver();
            _webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            _webDriver.Manage().Window.Maximize();
        }

        private void QuitWebDriver() {
            try {
                _webDriver.Quit();
            } catch (WebDriverException) {
                //ignore
            }
        }

        protected T On<T>() where T : class {
            return PageFactory.InitElementsIn<T>(_webDriver);
        }

        protected void ClearCookies() {
            _webDriver.Manage().Cookies.DeleteAllCookies();
        }

        protected void NavigateToUrl(string url = "http://www.justanswer.com/") {
            _webDriver.Navigate().GoToUrl(url);
        }

        [TestFixtureSetUp]
        public void FixtureSetUp() {
            InitWebDriver();
        }

        [TestFixtureTearDown]
        public void FixtuteTearDown() {
            QuitWebDriver();
            ;
        }

    }

}
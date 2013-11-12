using System;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Events;

using Selenium.HtmlElements.Factory;

namespace Selenium.HtmlElements.Demo.Tests {

    [TestFixture]
    public class BaseWebDriverTest {

        private IWebDriver _webDriver;

        private void InitWebDriver() {
            var eventFiringDriver = new EventFiringWebDriver(new FirefoxDriver());
            eventFiringDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            eventFiringDriver.Manage().Window.Maximize();
            
            eventFiringDriver.ElementClicked += (sender, args) => Console.WriteLine("{0} clicked", args.Element);
            eventFiringDriver.ExceptionThrown += (sender, args) => Console.WriteLine(args.ThrownException);
            eventFiringDriver.ScriptExecuted += (sender, args) => Console.WriteLine("JS executed: {0}", args.Script);
            eventFiringDriver.ScriptExecuting += (sender, args) => Console.WriteLine("executing JS: {0}", args.Script); 

            _webDriver = eventFiringDriver;
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
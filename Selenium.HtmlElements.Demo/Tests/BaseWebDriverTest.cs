using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.Events;

namespace HtmlElements.Demo.Tests
{
    [TestFixture]
    public class BaseWebDriverTest : AssertionHelper
    {
        private IWebDriver _webDriver;
        private readonly IPageObjectFactory PageObjectFactory = new DefaultPageObjectFactory();

        private void InitWebDriver()
        {
            var eventFiringDriver = new EventFiringWebDriver(new InternetExplorerDriver());
            eventFiringDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            eventFiringDriver.Manage().Window.Maximize();

            eventFiringDriver.ElementClicked += (sender, args) => Console.WriteLine("{0} clicked", args.Element);
            eventFiringDriver.ExceptionThrown += (sender, args) => Console.WriteLine(args.ThrownException);
            eventFiringDriver.ScriptExecuted += (sender, args) => Console.WriteLine("JS executed: {0}", args.Script);
            eventFiringDriver.ScriptExecuting += (sender, args) => Console.WriteLine("executing JS: {0}", args.Script);

            _webDriver = eventFiringDriver;
            _webDriver.Manage().Timeouts();
        }

        private void QuitWebDriver()
        {
            try
            {
                _webDriver.Quit();
            }
            catch (WebDriverException)
            {
                //ignore
            }
        }

        protected T On<T>() where T : class
        {
            return PageObjectFactory.Create<T>(_webDriver);
        }

        protected void ClearCookies()
        {
            _webDriver.Manage().Cookies.DeleteAllCookies();
        }

        protected void NavigateToUrl(string url = "http://developerslife.ru/")
        {
            _webDriver.Navigate().GoToUrl(url);
        }

        protected string CurrentUrl
        {
            get { return _webDriver.Url; }
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            InitWebDriver();
        }

        [TestFixtureTearDown]
        public void FixtuteTearDown()
        {
            QuitWebDriver();
        }
    }
}
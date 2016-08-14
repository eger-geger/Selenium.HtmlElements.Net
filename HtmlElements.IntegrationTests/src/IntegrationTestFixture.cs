using System;
using System.IO;
using HtmlElements.IntegrationTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace HtmlElements.IntegrationTests {

    public class IntegrationTestFixture : AssertionHelper {

        private const String InitialUrl = "PageAlpha.htm";

        protected readonly IPageObjectFactory PageFactory = new PageObjectFactory();

        protected IWebDriver WebDriver { get; private set; }

        protected PageAlpha PageAlpha { get; private set; }

        [SetUp]
        public void OpenTestPage() {
            WebDriver.Navigate().GoToUrl(new Uri(Path.GetFullPath(InitialUrl)).AbsoluteUri);
            PageAlpha = PageFactory.Create<PageAlpha>(WebDriver);
        }

        [TestFixtureSetUp]
        public void InitBrowser() {
            WebDriver = new PhantomJSDriver();
        }

        [TestFixtureTearDown]
        public void CloseBrowser() {
            WebDriver.Quit();
        }

    }

}
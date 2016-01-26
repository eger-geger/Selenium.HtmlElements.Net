using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace HtmlElements.IntegrationTests {

    public class IntegrationTestFixture : AssertionHelper {

        private const String TestPageName = "TestPage.htm";

        protected IWebDriver WebDriver { get; private set; }

        protected readonly IPageObjectFactory PageFactory = new PageObjectFactory();

        [SetUp]
        public void OpenTestPage() {
            WebDriver.Navigate().GoToUrl(Path.GetFullPath(TestPageName));
        }

        [TestFixtureSetUp]
        public void InitBrowser() {
            WebDriver = new FirefoxDriver();
        }

        [TestFixtureTearDown]
        public void CloseBrowser() {
            WebDriver.Quit();
        }

    }

}
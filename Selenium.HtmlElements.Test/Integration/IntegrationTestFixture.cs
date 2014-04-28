using System;
using System.IO;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace HtmlElements.Test.Integration {

    public class IntegrationTestFixture : AssertionHelper {

        protected const String TestPageName = "TestPage.htm";
        protected IWebDriver WebDriver { get; private set; }

        [SetUp]
        public void OpenTestPage() {
            WebDriver.Navigate().GoToUrl(Path.GetFullPath(TestPageName));
        }

        [TestFixtureSetUp]
        public void InitBrowser() {
            WebDriver = new ChromeDriver();
        }

        [TestFixtureTearDown]
        public void CloseBrowser() {
            WebDriver.Quit();
        }

    }

}
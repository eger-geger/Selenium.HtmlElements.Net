using System;
using System.IO;
using HtmlElements.IntegrationTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Firefox;

namespace HtmlElements.IntegrationTests {

    public class IntegrationTestFixture
    {
        private const String PageFileName = "PageAlpha.htm";

        protected readonly IPageObjectFactory PageFactory = new PageObjectFactory();

        protected IWebDriver WebDriver { get; private set; }

        protected PageAlpha PageAlpha { get; private set; }

        private string PageFileFullPath => Path.GetFullPath(
            Path.Combine(TestContext.CurrentContext.TestDirectory, PageFileName)
        );

        [SetUp]
        public void OpenTestPage()
        {
            WebDriver.Navigate().GoToUrl(new Uri(PageFileFullPath).AbsoluteUri);
            PageAlpha = PageFactory.Create<PageAlpha>(WebDriver);
        }

        [OneTimeSetUp]
        public void InitBrowser()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(this.GetType().Assembly.Location);
            var options = new FirefoxOptions();
            options.AddArgument("--headless");
            WebDriver = new FirefoxDriver(options);
        }

        [OneTimeTearDown]
        public void CloseBrowser()
        {
            WebDriver.Quit();
        }

    }

}
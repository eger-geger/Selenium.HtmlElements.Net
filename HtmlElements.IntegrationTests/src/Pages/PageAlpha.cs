using System.Collections.Generic;
using HtmlElements.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace HtmlElements.IntegrationTests.Pages {

    public class PageAlpha : HtmlPage {

        public PageAlpha(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper) {}

        [FindsBy(How = How.CssSelector, Using = "#element-list")]
        public HtmlElement ElementListContainer { get; private set; }

        [FindsBy(How = How.CssSelector, Using = "#element-list li")]
        public IList<IWebElement> ElementListItems { get; private set; }

        [FindsBy(How = How.CssSelector, Using = "#element-list li"), CacheLookup]
        public IList<HtmlElement> CachedElementListItems { get; private set; } 

        [FindsBy(How = How.CssSelector, Using = "iframe"), CacheLookup]
        public PageBeta BetaFrame { get; set; }
    }

}
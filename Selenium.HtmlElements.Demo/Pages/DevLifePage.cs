using HtmlElements.Demo.Elements;
using HtmlElements.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace HtmlElements.Demo.Pages {

    internal class DevLifePage : HtmlPage {

        [FindsBy(How = How.CssSelector, Using = ".jslink:nth-child(1)"), CacheLookup]
        private HtmlLink _sortByDate;

        [FindsBy(How = How.CssSelector, Using = ".jslink:nth-child(3)"), CacheLookup]
        private HtmlLink _sortByHottest;

        [FindsBy(How = How.CssSelector, Using = ".jslink:nth-child(2)"), CacheLookup]
        private HtmlLink _sortByRatig;

        public DevLifePage(ISearchContext webElement) : base(webElement) {}

        [FindsBy(How = How.CssSelector, Using = ".entry")]
        public DevLifePost Posts { get; private set; }

        [FindsBy(How = How.CssSelector, Using = ".pagination"), CacheLookup]
        public Pagination Pagination { get; private set; }

        public void SortByDate() {
            _sortByDate.Click();
        }

        public void SortByRating() {
            _sortByRatig.Click();
        }

        public void SortByHottest() {
            _sortByHottest.Click();
        }

    }

}
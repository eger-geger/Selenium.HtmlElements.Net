using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using Selenium.HtmlElements.Demo.Elements;
using Selenium.HtmlElements.Elements;

namespace Selenium.HtmlElements.Demo.Pages {

    internal class DevLifePage : CustomElement {

        [FindsBy(How = How.CssSelector, Using = ".jslink:nth-child(1)")] private HtmlLink _sortByDate;

        [FindsBy(How = How.CssSelector, Using = ".jslink:nth-child(3)")] private HtmlLink _sortByHottest;

        [FindsBy(How = How.CssSelector, Using = ".jslink:nth-child(2)")] private HtmlLink _sortByRatig;

        public DevLifePage(ISearchContext wrapped) : base(wrapped) {}

        [FindsBy(How = How.CssSelector, Using = ".entry")]
        public DevLifePost Posts { get; private set; }

        [FindsBy(How = How.CssSelector, Using = ".pagination")]
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
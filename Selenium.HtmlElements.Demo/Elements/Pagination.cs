using HtmlElements.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace HtmlElements.Demo.Elements
{
    internal class Pagination : HtmlElement
    {
        [FindsBy(How = How.CssSelector, Using = ".value.rating")]
        private HtmlElement _currentPageNumber;

        [FindsBy(How = How.CssSelector, Using = ".nextPage")]
        private HtmlLink _nextPageLink;

        public Pagination(IWebElement wrapped) : base(wrapped)
        {
        }

        public int CurrentNumber
        {
            get { return int.Parse(_currentPageNumber.Text); }
        }

        public void OpenNextPage()
        {
            _nextPageLink.Click();
        }
    }
}
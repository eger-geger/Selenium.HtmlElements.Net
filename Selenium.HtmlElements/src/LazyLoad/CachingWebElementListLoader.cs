using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class CachingWebElementListLoader : CachingLoader<ReadOnlyCollection<IWebElement>>
    {
        private readonly By _locator;

        private readonly ISearchContext _searchContext;

        public CachingWebElementListLoader(ISearchContext searchContext, By locator)
        {
            _searchContext = searchContext;
            _locator = locator;
        }

        protected override ReadOnlyCollection<IWebElement> ExecuteLoad()
        {
            return _searchContext.FindElements(_locator);
        }
    }
}
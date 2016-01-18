using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class CachingWebElementLoader : CachingLoader<IWebElement>
    {
        private readonly By _locator;

        private readonly ISearchContext _searchContext;

        public CachingWebElementLoader(ISearchContext searchContext, By locator)
        {
            _searchContext = searchContext;
            _locator = locator;
        }

        protected override IWebElement ExecuteLoad()
        {
            return _searchContext.FindElement(_locator);
        }
    }
}
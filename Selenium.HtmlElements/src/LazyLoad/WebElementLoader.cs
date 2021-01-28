using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class WebElementLoader : CachingLoader<IWebElement>
    {
        private readonly By _locator;

        public WebElementLoader(ISearchContext searchContext, By locator, bool enableCache) : base(enableCache)
        {
            _locator = locator;

            SearchContext = searchContext;
        }

        public override ISearchContext SearchContext { get; }

        protected override IWebElement ExecuteLoad()
        {
            return SearchContext.FindElement(_locator);
        }

        public override string ToString()
        {
            return string.Format("{0} locating element using [{1}] in [{2}]", GetType().Name, _locator, SearchContext);
        }
    }
}
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class TransparentWebElementLoader : TransparentLoader<IWebElement>
    {
        private readonly By _locator;

        private readonly ISearchContext _searchContext;

        public TransparentWebElementLoader(ISearchContext searchContext, By locator)
        {
            _searchContext = searchContext;
            _locator = locator;
        }

        public override IWebElement Load()
        {
            return _searchContext.FindElement(_locator);
        }
    }
}
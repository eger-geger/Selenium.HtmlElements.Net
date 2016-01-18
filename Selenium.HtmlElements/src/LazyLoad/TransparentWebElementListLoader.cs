using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class TransparentWebElementListLoader : TransparentLoader<ReadOnlyCollection<IWebElement>>
    {
        private readonly By _locator;

        private readonly ISearchContext _searchContext;

        public TransparentWebElementListLoader(By locator, ISearchContext searchContext)
        {
            _locator = locator;
            _searchContext = searchContext;
        }

        public override ReadOnlyCollection<IWebElement> Load()
        {
            return _searchContext.FindElements(_locator);
        }
    }
}
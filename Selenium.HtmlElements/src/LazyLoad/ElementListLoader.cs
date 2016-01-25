using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class ElementListLoader : CachingLoader<ReadOnlyCollection<IWebElement>>
    {
        private readonly By _locator;

        private readonly ISearchContext _searchContext;

        public ElementListLoader(ISearchContext searchContext, By locator, Boolean enableCache) : base(enableCache)
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
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

        public override string ToString()
        {
            return String.Format("Loader for list of elements located by [{0}] in [{1}]", _locator, _searchContext);
        }
    }
}
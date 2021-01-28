using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class WebElementListLoader : CachingLoader<ReadOnlyCollection<IWebElement>>
    {
        private readonly By _locator;


        public WebElementListLoader(ISearchContext searchContext, By locator, bool enableCache) : base(enableCache)
        {
            _locator = locator;
            SearchContext = searchContext;
        }

        public override ISearchContext SearchContext { get; }

        protected override ReadOnlyCollection<IWebElement> ExecuteLoad()
        {
            return SearchContext.FindElements(_locator);
        }

        public override string ToString()
        {
            return string.Format("{0} providing list of elements found with [{1}] in [{2}]", GetType().Name, _locator,
                SearchContext);
        }
    }
}
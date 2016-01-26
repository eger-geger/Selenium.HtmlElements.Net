using System;
using System.Collections.ObjectModel;
using System.Text;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class WebElementListLoader : CachingLoader<ReadOnlyCollection<IWebElement>>
    {
        private readonly By _locator;

        private readonly ISearchContext _searchContext;

        public WebElementListLoader(ISearchContext searchContext, By locator, Boolean enableCache) : base(enableCache)
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
            return new StringBuilder()
                .AppendFormat("{0} providing list of elements found with [{1}] in", GetType().FullName, _locator)
                .AppendLine()
                .AppendLine(_searchContext.ToString().ShiftLinesToRight(2, '.'))
                .ToString();
        }
    }
}
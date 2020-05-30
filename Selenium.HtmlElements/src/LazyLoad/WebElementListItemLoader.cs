using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class WebElementListItemLoader : CachingLoader<IWebElement>
    {
        private readonly int _index;

        private readonly ILoader<ReadOnlyCollection<IWebElement>> _listLoader;

        public WebElementListItemLoader(ILoader<ReadOnlyCollection<IWebElement>> listLoader, int index, IWebElement value = null) : base(true, value)
        {
            _listLoader = listLoader;
            _index = index;
        }

        protected override IWebElement ExecuteLoad()
        {
            try
            {
                return _listLoader.Load()[_index];
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new NoSuchElementException(string.Format("List element [{0}] not found in [{1}]", _index,_listLoader), ex);
            }
            
        }

        public override void Reset()
        {
            base.Reset();
            _listLoader.Reset();
        }

        public override ISearchContext SearchContext => _listLoader.SearchContext;

        public override string ToString()
        {
            return string.Format("{0} providing [{1}] element from the list loaded by [{2}]", GetType().Name, _index, _listLoader);
        }
    }
}
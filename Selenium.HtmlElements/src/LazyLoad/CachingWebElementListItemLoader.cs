using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class CachingWebElementListItemLoader : CachingLoader<IWebElement>
    {
        private readonly Int32 _index;
        private readonly ILoader<ReadOnlyCollection<IWebElement>> _listLoader;

        public CachingWebElementListItemLoader(
            ILoader<ReadOnlyCollection<IWebElement>> listLoader, 
            Int32 index,
            IWebElement initialValue = null) : base(initialValue)
        {
            _listLoader = listLoader;
            _index = index;
        }

        protected override IWebElement ExecuteLoad()
        {
            return _listLoader.Load()[_index];
        }

        public override void Reset()
        {
            base.Reset();
            _listLoader.Reset();
        }
    }
}
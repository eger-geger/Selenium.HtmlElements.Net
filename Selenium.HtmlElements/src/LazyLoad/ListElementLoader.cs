using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class ListElementLoader : CachingLoader<IWebElement>
    {
        private readonly Int32 _index;

        private readonly ILoader<ReadOnlyCollection<IWebElement>> _listLoader;

        public ListElementLoader(ILoader<ReadOnlyCollection<IWebElement>> listLoader, Int32 index, IWebElement value = null) : base(true, value)
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
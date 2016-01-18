using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HtmlElements.Proxy;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class CachingTypedListLoader<TElement> : CachingLoader<IList<TElement>>
    {
        private readonly ILoader<ReadOnlyCollection<IWebElement>> _elementListLoader;
        private readonly IPageObjectFactory _pageObjectFactory;
        private readonly IProxyFactory _proxyFactory;

        public CachingTypedListLoader(
            ILoader<ReadOnlyCollection<IWebElement>> elementListLoader,
            IPageObjectFactory pageObjectFactory, 
            IProxyFactory proxyFactory)
        {
            _elementListLoader = elementListLoader;
            _pageObjectFactory = pageObjectFactory;
            _proxyFactory = proxyFactory;
        }

        public override void Reset()
        {
            base.Reset();

            _elementListLoader.Reset();
        }

        protected override IList<TElement> ExecuteLoad()
        {
            return _elementListLoader.Load().Select(CreateTypedElement).ToList();
        }

        private TElement CreateTypedElement(IWebElement element, Int32 index)
        {
            var loader = new CachingWebElementListItemLoader(_elementListLoader, index, element);

            return _pageObjectFactory.Create<TElement>(_proxyFactory.CreateWebElementProxy(loader));
        }
    }
}
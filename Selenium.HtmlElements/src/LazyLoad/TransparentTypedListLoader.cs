using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HtmlElements.Proxy;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class TransparentTypedListLoader<TElement> : TransparentLoader<IList<TElement>>
    {
        private readonly ILoader<ReadOnlyCollection<IWebElement>> _elementListLoader;
        private readonly IPageObjectFactory _pageObjectFactory;
        private readonly IProxyFactory _proxyFactory;

        public TransparentTypedListLoader(
            ILoader<ReadOnlyCollection<IWebElement>> elementListLoader,
            IPageObjectFactory pageObjectFactory,
            IProxyFactory proxyFactory)
        {
            _elementListLoader = elementListLoader;
            _pageObjectFactory = pageObjectFactory;
            _proxyFactory = proxyFactory;
        }

        public override IList<TElement> Load()
        {
            return _elementListLoader.Load().Select(CreateTypedElement).ToList();
        }

        private TElement CreateTypedElement(IWebElement element, Int32 index)
        {
            var loader = new CachingWebElementListItemLoader(_elementListLoader, index, element);

            return _pageObjectFactory.Create<TElement>(_proxyFactory.CreateElementProxy(loader));
        }
    }
}
using System;
using System.Collections.ObjectModel;
using HtmlElements.Proxy;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal class ElementLoaderFactory
    {
        private readonly IPageObjectFactory _pageObjectFactory;
        private readonly IProxyFactory _proxyFactory;

        public ElementLoaderFactory(IPageObjectFactory pageObjectFactory, IProxyFactory proxyFactory)
        {
            _pageObjectFactory = pageObjectFactory;
            _proxyFactory = proxyFactory;
        }

        public ILoader<IWebElement> CreateElementLoader(ISearchContext searchContext, By elementLocator, Boolean cached)
        {
            if (cached)
            {
                return new CachingWebElementLoader(searchContext, elementLocator);
            }
            return new TransparentWebElementLoader(searchContext, elementLocator);
        }

        public ILoader<ReadOnlyCollection<IWebElement>> CreateWebElementListLoader(ISearchContext searchContext,
            By elementLocator, Boolean cached)
        {
            if (cached)
            {
                return new CachingWebElementListLoader(searchContext, elementLocator);
            }
            return new TransparentWebElementListLoader(elementLocator, searchContext);
        }

        public Object CreateTypedListLoader(Type elementType, ILoader<ReadOnlyCollection<IWebElement>> elementLoader,
            Boolean cached)
        {
            var listGenericType = cached
                ? typeof (CachingTypedListLoader<>)
                : typeof (TransparentTypedListLoader<>);

            return Activator.CreateInstance(
                listGenericType.MakeGenericType(elementType),
                elementLoader,
                _pageObjectFactory,
                _proxyFactory);
        }

        public Object CreateTypedListLoader(Type elementType, ISearchContext searchContext, By elementLocator,
            Boolean cached)
        {
            return CreateTypedListLoader(elementType, CreateWebElementListLoader(searchContext, elementLocator, cached),
                cached);
        }
    }
}
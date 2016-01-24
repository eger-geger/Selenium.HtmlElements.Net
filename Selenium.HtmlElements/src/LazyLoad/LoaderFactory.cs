using System;
using System.Collections.ObjectModel;
using HtmlElements.Proxy;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    public class LoaderFactory : ILoaderFactory
    {
        private readonly IPageObjectFactory _pageObjectFactory;
        private readonly IProxyFactory _proxyFactory;

        public LoaderFactory(IPageObjectFactory pageObjectFactory, IProxyFactory proxyFactory)
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

        public ILoader<ReadOnlyCollection<IWebElement>> CreateElementListLoader(ISearchContext searchContext,
            By elementLocator, Boolean cached)
        {
            if (cached)
            {
                return new CachingWebElementListLoader(searchContext, elementLocator);
            }

            return new TransparentWebElementListLoader(elementLocator, searchContext);
        }

        public Object CreateListLoader(Type elementType, ILoader<ReadOnlyCollection<IWebElement>> elementLoader, Boolean cached)
        {
            var loaderGenericType = cached
                ? typeof (CachingTypedListLoader<>)
                : typeof (TransparentTypedListLoader<>);

            var loaderType = loaderGenericType.MakeGenericType(elementType);

            return Activator.CreateInstance(loaderType, elementLoader, _pageObjectFactory, _proxyFactory);
        }

        public Object CreateListLoader(Type elementType, ISearchContext searchContext, By elementLocator, Boolean cached)
        {
            return CreateListLoader(elementType, CreateElementListLoader(searchContext, elementLocator, cached), cached);
        }
    }
}
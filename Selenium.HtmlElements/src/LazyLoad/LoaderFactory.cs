using System;
using System.Collections.Generic;
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

        public ILoader<IWebElement> CreateElementLoader(ISearchContext searchContext, By elementLocator, Boolean enableCache)
        {
            return new WebElementLoader(searchContext, elementLocator, enableCache);
        }

        public ILoader<ReadOnlyCollection<IWebElement>> CreateElementListLoader(ISearchContext searchContext, By elementLocator, Boolean enableCache)
        {
            return new WebElementListLoader(searchContext, elementLocator, enableCache);
        }

        public Object CreateListLoader(Type elementType, ILoader<ReadOnlyCollection<IWebElement>> elementLoader, Boolean enableCache)
        {
            return Activator.CreateInstance(
                typeof(TypedElementListLoader<>).MakeGenericType(elementType), 
                elementLoader, _pageObjectFactory, _proxyFactory, enableCache
            );
        }

        public Object CreateListLoader(Type elementType, ISearchContext searchContext, By elementLocator, Boolean enableCache)
        {
            return CreateListLoader(elementType, CreateElementListLoader(searchContext, elementLocator, enableCache), enableCache);
        }

        public ILoader<IList<TElement>> CreateListLoader<TElement>(ILoader<ReadOnlyCollection<IWebElement>> elementLoader, Boolean enableCache)
        {
            return CreateListLoader(typeof(TElement), elementLoader, enableCache) as ILoader<IList<TElement>>;
        }

        public ILoader<IList<TElement>> CreateListLoader<TElement>(ISearchContext searchContext, By elementLocator, Boolean enableCache)
        {
            return CreateListLoader(typeof(TElement), searchContext, elementLocator, enableCache) as ILoader<IList<TElement>>;
        }
    }
}
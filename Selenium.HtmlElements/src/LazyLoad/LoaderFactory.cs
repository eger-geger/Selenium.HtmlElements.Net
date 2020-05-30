using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HtmlElements.Proxy;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    /// <summary>
    ///     Creates caching loaders for WebElements and list of WebElements.
    /// </summary>
    /// <seealso cref="HtmlElements.LazyLoad.ILoaderFactory" />
    public class LoaderFactory : ILoaderFactory
    {
        private readonly IPageObjectFactory _pageObjectFactory;
        private readonly IProxyFactory _proxyFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoaderFactory"/> class.
        /// </summary>
        /// <param name="pageObjectFactory">
        ///     The page object factory used for creating typed list items dynamically.
        /// </param>
        /// <param name="proxyFactory">
        ///     The proxy factory used for creating typed list items dynamically.
        /// </param>
        public LoaderFactory(IPageObjectFactory pageObjectFactory, IProxyFactory proxyFactory)
        {
            if (pageObjectFactory == null)
            {
                throw new ArgumentNullException(nameof(pageObjectFactory));
            }

            if (proxyFactory == null)
            {
                throw new ArgumentNullException(nameof(proxyFactory));
            }

            _pageObjectFactory = pageObjectFactory;
            _proxyFactory = proxyFactory;
        }


        /// <summary>
        ///     Creates element loader using search context and locator to find WebElement.
        /// </summary>
        /// <param name="searchContext">
        ///     Where to search for web element.
        /// </param>
        /// <param name="locator">
        ///     How to find a web element.
        /// </param>
        /// <param name="enableCache">
        ///     Whether element should be cached once found.
        /// </param>
        /// <returns>
        ///     Loader instance.
        /// </returns>
        public ILoader<IWebElement> CreateElementLoader(ISearchContext searchContext, By locator, bool enableCache)
        {
            return new WebElementLoader(searchContext, locator, enableCache);
        }


        /// <summary>
        ///     Creates element list loader using search context and locator to find list of elements.
        /// </summary>
        /// <param name="searchContext">
        ///     Where to search for web elements.
        /// </param>
        /// <param name="locator">
        ///     How to find a web elements.
        /// </param>
        /// <param name="enableCache">
        ///     Whether elements should be cached once found.
        /// </param>
        /// <returns>
        ///     Loader instance.
        /// </returns>
        public ILoader<ReadOnlyCollection<IWebElement>> CreateElementListLoader(ISearchContext searchContext, By locator, bool enableCache)
        {
            return new WebElementListLoader(searchContext, locator, enableCache);
        }

    
        /// <summary>
        ///     Create list loader wrapping web elements loader.
        /// </summary>
        /// <param name="elementType">
        ///     Type of list items.
        /// </param>
        /// <param name="searchContext">
        ///     Where to search for web elements.
        /// </param>
        /// <param name="locator">
        ///     How to find a web elements.
        /// </param>
        /// <param name="enableCache">
        ///     Whether elements should be cached once found.
        /// </param>
        /// <returns>
        ///     Loader instance.
        /// </returns>
        public object CreateListLoader(Type elementType, ISearchContext searchContext, By locator, bool enableCache)
        {
            return Activator.CreateInstance(
                typeof(TypedElementListLoader<>).MakeGenericType(elementType),
                CreateElementListLoader(searchContext, locator, enableCache), 
                _pageObjectFactory, 
                _proxyFactory, 
                enableCache
            );
        }

        /// <summary>
        ///     Create list loader wrapping web elements loader.
        /// </summary>
        /// <typeparam name="TElement">
        ///     Type of list items.
        /// </typeparam>
        /// <param name="searchContext">
        ///     Context used to locate web elements.
        /// </param>
        /// <param name="locator">
        ///     How to find a web elements.
        /// </param>
        /// <param name="enableCache">
        ///     Whether elements should be cached once found.
        /// </param>
        /// <returns>
        ///     Loader instance.
        /// </returns>
        public ILoader<IList<TElement>> CreateListLoader<TElement>(ISearchContext searchContext, By locator, bool enableCache)
        {
            return CreateListLoader(typeof(TElement), searchContext, locator, enableCache) as ILoader<IList<TElement>>;
        }
    }
}
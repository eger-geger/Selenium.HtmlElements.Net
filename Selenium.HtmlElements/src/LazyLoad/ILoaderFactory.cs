using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    /// <summary>
    ///     Creates loaders for web elements and element lists
    /// </summary>
    public interface ILoaderFactory
    {
        /// <summary>
        ///     Creates element loader using search context and locator to find element when needed
        /// </summary>
        /// <param name="searchContext">Where to search for web element</param>
        /// <param name="elementLocator">How to find a web element</param>
        /// <param name="cached">Whether element should be cached once found</param>
        /// <returns>Loader instance</returns>
        ILoader<IWebElement> CreateElementLoader(ISearchContext searchContext, By elementLocator, Boolean cached);

        /// <summary>
        ///     Creates element list loader using search context and locator to find list of elements when needed
        /// </summary>
        /// <param name="searchContext">Where to search for web elements</param>
        /// <param name="elementLocator">How to find a web elements</param>
        /// <param name="cached">Whether elements should be cached once found</param>
        /// <returns>Loader instance</returns>
        ILoader<ReadOnlyCollection<IWebElement>> CreateElementListLoader(ISearchContext searchContext, By elementLocator, Boolean cached);

        /// <summary>
        ///     Create list loader wrapping web elements loader
        /// </summary>
        /// <param name="elementType">Type of list items</param>
        /// <param name="elementLoader">Loader providing list of raw web elements</param>
        /// <param name="cached">Whether elements should be cached once found</param>
        /// <returns>Loader instance</returns>
        Object CreateListLoader(Type elementType, ILoader<ReadOnlyCollection<IWebElement>> elementLoader, Boolean cached);

        /// <summary>
        ///     Create list loader wrapping web elements loader
        /// </summary>
        /// <param name="elementType">Type of list items</param>
        /// <param name="searchContext">Where to search for web elements</param>
        /// <param name="elementLocator">How to find a web elements</param>
        /// <param name="cached">Whether elements should be cached once found</param>
        /// <returns>Loader instance</returns>
        Object CreateListLoader(Type elementType, ISearchContext searchContext, By elementLocator, Boolean cached);
    }
}
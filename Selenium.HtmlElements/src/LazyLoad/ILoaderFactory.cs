using System;
using System.Collections.Generic;
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
        ILoader<IWebElement> CreateElementLoader(ISearchContext searchContext, By locator, Boolean enableCache);

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
        ILoader<ReadOnlyCollection<IWebElement>> CreateElementListLoader(ISearchContext searchContext, By locator, Boolean enableCache);

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
        Object CreateListLoader(Type elementType, ISearchContext searchContext, By locator, Boolean enableCache);

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
        ILoader<IList<TElement>> CreateListLoader<TElement>(ISearchContext searchContext, By locator, Boolean enableCache);
    }
}
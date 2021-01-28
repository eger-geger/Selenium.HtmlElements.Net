using System;
using System.Collections.Generic;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;

namespace HtmlElements.Proxy
{
    /// <summary>
    ///     Creates instances implementing <see cref="IWebElement"/> or <see cref="IList{T}"/> 
    ///     interfaces and wrapping corresponding element loaders
    /// </summary>
    public interface IProxyFactory
    {
        /// <summary>
        ///     Creates <see cref="IWebElement"/> instance using <paramref name="loader"/> to get raw <see cref="IWebElement"/> and delegating calls to it.
        /// </summary>
        /// <param name="loader">
        ///     Element loader providing raw <see cref="IWebElement"/>.
        /// </param>
        /// <returns>
        ///     Proxy implementing <see cref="IWebElement"/> interface.
        /// </returns>
        IWebElement CreateWebElementProxy(ILoader<IWebElement> loader);

        /// <summary>
        ///     Create frame-specific web element proxy using <see cref="IWebDriver"/> to locate nested elements.
        /// </summary>
        /// <param name="loader">
        ///     Element loader providing raw <see cref="IWebElement"/> pointing to frame.
        /// </param>
        /// <returns>
        ///     Proxy implementing <see cref="IWebElement"/> interface.
        /// </returns>
        IWebElement CreateFrameProxy(ILoader<IWebElement> loader);

        /// <summary>
        ///     Creates list of WebElements wrapping <paramref name="loader"/> and delegating all calls to list returned by it.
        /// </summary>
        /// <param name="elementType">
        ///     Type of elements stored in list.
        /// </param>
        /// <param name="loader">
        ///     List loader providing collection of raw <see cref="IWebElement">WebElements</see> or page objects.
        /// </param>
        /// <returns>
        ///     Proxy implementing <see cref="IList{T}"/> interface.
        /// </returns>
        object CreateListProxy(Type elementType, object loader);

        /// <summary>
        ///     Creates list of WebElements wrapping <paramref name="loader"/> and delegating all calls to list returned by it.
        /// </summary>
        /// <typeparam name="TElement">
        ///     Type of elements stored in list.
        /// </typeparam>
        /// <param name="loader">
        ///     List loader providing collection of raw <see cref="IWebElement">WebElements</see> or page objects.
        /// </param>
        /// <returns>
        ///     Proxy implementing <see cref="IList{T}"/> interface.
        /// </returns>
        IList<TElement> CreateListProxy<TElement>(ILoader<IList<TElement>> loader);
    }
}
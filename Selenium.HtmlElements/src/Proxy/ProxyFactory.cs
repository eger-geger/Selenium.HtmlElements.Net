using System;
using System.Collections.Generic;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;

namespace HtmlElements.Proxy
{
    /// <summary>
    ///     Default proxy factory using hand-crafted proxies
    /// </summary>
    public class ProxyFactory : IProxyFactory
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
        /// <exception cref="ArgumentNullException">
        ///     Thrown when provided loader is null.
        /// </exception>
        public IWebElement CreateWebElementProxy(ILoader<IWebElement> loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }

            return new WebElementProxy(loader);
        }

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
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="elementType"/> or <paramref name="loader"/> is null.
        /// </exception>
        public object CreateListProxy(Type elementType, object loader)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException(nameof(elementType));
            }

            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }

            var expectedLoaderType = typeof(ILoader<>).MakeGenericType(typeof(IList<>).MakeGenericType(elementType));

            if (!expectedLoaderType.IsInstanceOfType(loader))
            {
                throw new ArgumentException(
                    string.Format("Wrong loader type: expected [{0}] but was [{1}]", expectedLoaderType, loader.GetType()), nameof(loader)
                );
            }

            return Activator.CreateInstance(typeof (ElementListProxy<>).MakeGenericType(elementType), loader);
        }

        /// <summary>
        ///     Create frame-specific web element proxy using <see cref="IWebDriver"/> to locate nested elements.
        /// </summary>
        /// <param name="loader">
        ///     Element loader providing raw <see cref="IWebElement"/> pointing to frame.
        /// </param>
        /// <returns>
        ///     Proxy implementing <see cref="IWebElement"/> interface.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when provided loader is null.
        /// </exception>
        public IWebElement CreateFrameProxy(ILoader<IWebElement> loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }

            return new FrameWebElementProxy(loader);
        }

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
        /// <exception cref="ArgumentNullException">
        ///     Thrown when provided loader is null.
        /// </exception>
        public IList<TElement> CreateListProxy<TElement>(ILoader<IList<TElement>> loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }

            return new ElementListProxy<TElement>(loader);
        }
    }
}
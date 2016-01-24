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
        ///     Creates web element instance wrapping element loader
        /// </summary>
        /// <param name="loader">Element loader</param>
        /// <returns>Proxy implementing <see cref="IWebElement"/> interface</returns>
        IWebElement CreateElementProxy(ILoader<IWebElement> loader);

        /// <summary>
        ///     Creates list of web elements wrapping given loader
        /// </summary>
        /// <param name="elementType">Type of elements stored in list</param>
        /// <param name="loader">List loader</param>
        /// <returns>Proxy implementing <see cref="IList{T}"/> interface</returns>
        Object CreateListProxy(Type elementType, Object loader);
        
        /// <summary>
        ///     Creates list of web elements wrapping given loader
        /// </summary>
        /// <typeparam name="TElement">Type of elements stored in list</typeparam>
        /// <param name="loader">List loader</param>
        /// <returns>Proxy implementing <see cref="IList{T}"/> interface</returns>
        IList<TElement> CreateListProxy<TElement>(ILoader<IList<TElement>> loader);
    }
}
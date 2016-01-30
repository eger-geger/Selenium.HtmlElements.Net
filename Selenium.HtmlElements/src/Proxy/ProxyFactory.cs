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
        public IWebElement CreateElementProxy(ILoader<IWebElement> elementLoader)
        {
            if (elementLoader == null)
            {
                throw new ArgumentNullException("elementLoader");
            }

            return new WebElementProxy(elementLoader);
        }

        public Object CreateListProxy(Type elementType, Object loader)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }

            if (loader == null)
            {
                throw new ArgumentNullException("loader");
            }

            var expectedListType = typeof (IList<>).MakeGenericType(elementType);
            var expectedLoaderType = typeof (ILoader<>).MakeGenericType(expectedListType);

            if (!expectedLoaderType.IsInstanceOfType(loader))
            {
                throw new ArgumentException(
                    String.Format(
                        "Wrong loader type: expected [{0}] but was [{1}]",
                        expectedLoaderType, loader.GetType()
                        ), "loader"
                    );
            }

            return Activator.CreateInstance(typeof (ElementListProxy<>).MakeGenericType(elementType), loader);
        }

        public IWebElement CreateFrameProxy(ILoader<IWebElement> elementLoader)
        {
            if (elementLoader == null)
            {
                throw new ArgumentNullException("elementLoader");
            }

            return new FrameWebElementProxy(elementLoader);
        }

        public IList<TElement> CreateListProxy<TElement>(ILoader<IList<TElement>> loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException("loader");
            }

            return new ElementListProxy<TElement>(loader);
        }
    }
}
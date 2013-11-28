using System;
using System.Collections.Generic;

using Castle.Core.Interceptor;
using Castle.DynamicProxy;

using OpenQA.Selenium;

using Selenium.HtmlElements.Elements;
using Selenium.HtmlElements.Internal;

using log4net;

namespace Selenium.HtmlElements {

    public static class ElementFactory {

        private static readonly ProxyGenerator ProxyFactory = new ProxyGenerator();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ElementFactory));

        public static IList<T> CreateElementList<T>(IElementLocator locator, bool useCash = false) where T : class, IWebElement {
            return Create(typeof(IList<T>), locator) as IList<T>;
        }

        public static T CreateElement<T>(IElementLocator locator, bool useCash = true) where T : class, IWebElement {
            return Create(typeof(T), locator, useCash) as T;
        }

        public static Object Create(Type type, IElementLocator locator, bool useCash = true) {
            if (type.IsWebElement()) return NewElement(type, locator, useCash);

            if (type.IsWebElementList()) return NewCollection(type.GetGenericArguments()[0], locator, useCash);

            throw new InvalidOperationException(string.Format("Cannot create instance of {0}", type));
        }

        private static object NewElement(Type elementType, IElementLocator elementLocator, bool useCash) {
            var proxyElement = GenerateProxy(typeof(IHtmlElement), new WebElementProxy(elementLocator, useCash)) as IHtmlElement;

            if (elementType == typeof(IHtmlElement) || elementType == typeof(IWebElement)) return new HtmlElement(proxyElement);

            return PageObjectFactory.Create(elementType, proxyElement);
        }

        private static object NewCollection(Type elementType, IElementLocator elementLocator, bool useCash) {
            Logger.DebugFormat("creating list of {0}", elementType);

            try {
                return GenerateProxy(typeof(IList<>).MakeGenericType(elementType),
                    new ElementListProxy(elementType, elementLocator, useCash));
            } catch (Exception) {
                Logger.FatalFormat("failed to create list of {0}", elementType);

                throw;
            }
        }

        private static object GenerateProxy(Type interfaceToProxy, IInterceptor interceptor) {
            return ProxyFactory.CreateInterfaceProxyWithoutTarget(interfaceToProxy, interceptor);
        }

    }

}
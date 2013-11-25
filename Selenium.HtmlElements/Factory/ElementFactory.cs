using System;
using System.Collections.Generic;

using Castle.Core.Interceptor;
using Castle.DynamicProxy;

using OpenQA.Selenium;

using Selenium.HtmlElements.Elements;
using Selenium.HtmlElements.Locators;
using Selenium.HtmlElements.Proxy;

namespace Selenium.HtmlElements.Factory {

    public static class ElementFactory {

        private static readonly ProxyGenerator ProxyFactory = new ProxyGenerator();

        public static IList<T> CreateElementList<T>(IElementLocator locator) where T : class, IWebElement {
            return Create(typeof(IList<T>), locator) as IList<T>;
        }

        public static T CreateElement<T>(IElementLocator locator) where T : class, IWebElement {
            return Create(typeof(T), locator) as T;
        }

        public static Object Create(Type type, IElementLocator locator) {
            if (type.IsWebElementList()) return NewCollection(type.GetGenericArguments()[0], locator);

            if (type.IsWebElement()) return NewElement(type, locator);

            throw new InvalidOperationException(string.Format("Cannot create instance of {0}", type));
        }

        private static object NewElement(Type elementType, IElementLocator elementLocator) {
            var proxyElement = GenerateProxy(typeof(IHtmlElement), new WebElementProxy(elementLocator)) as IHtmlElement;

            if (elementType == typeof(IHtmlElement) || elementType == typeof(IWebElement))
                return new HtmlElement(proxyElement);

            return PageObjectFactory.Create(elementType, proxyElement);
        }

        private static object NewCollection(Type elementType, IElementLocator elementLocator) {
            return GenerateProxy(typeof(IList<>).MakeGenericType(elementType),
                new ElementListProxy(elementType, elementLocator));
        }

        private static object GenerateProxy(Type interfaceToProxy, IInterceptor interceptor) {
            return ProxyFactory.CreateInterfaceProxyWithoutTarget(interfaceToProxy, interceptor);
        }

    }

}
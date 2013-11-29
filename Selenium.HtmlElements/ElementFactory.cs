using System;
using System.Collections.Generic;

using Castle.Core.Interceptor;
using Castle.DynamicProxy;

using HtmlElements.Elements;
using HtmlElements.Locators;
using HtmlElements.Proxy;

using OpenQA.Selenium;

using HtmlElements.Extensions;

using log4net;

namespace HtmlElements {

    public static class ElementFactory {

        private static readonly ProxyGenerator ProxyFactory = new ProxyGenerator();

        public static IList<T> CreateElementList<T>(IElementLocator locator, bool useCache = false) where T : class, IWebElement {
            return Create(typeof(IList<T>), locator, useCache) as IList<T>;
        }

        public static T CreateElement<T>(IElementLocator locator, bool useCache = true) where T : class, IWebElement {
            return Create(typeof(T), locator, useCache) as T;
        }

        public static Object Create(Type type, IElementLocator locator, bool useCache = false) {
            if (type.IsWebElement()) return NewElement(type, locator, useCache);
            if (type.IsWebElementList()) return NewElementList(type.GetGenericArguments()[0], locator, useCache);

            throw new InvalidOperationException(string.Format("Cannot create instance of [{0}]", type));
        }

        private static object NewElement(Type type, IElementLocator locator, bool useCash) {
            var proxy = GenerateProxy(typeof(IHtmlElement), new ElementProxy(locator, useCash)) as IHtmlElement;

            return type == typeof(IHtmlElement) || type == typeof(IWebElement)
                ? new HtmlElement(proxy) : ObjectFactory.Create(type, proxy);
        }

        private static object NewElementList(Type type, IElementLocator locator, bool useCash) {
            return GenerateProxy(typeof(IList<>).MakeGenericType(type), new ElementListProxy(type, locator, useCash));
        }

        private static object GenerateProxy(Type @interface, IInterceptor interceptor) {
            return ProxyFactory.CreateInterfaceProxyWithoutTarget(@interface, interceptor);
        }

    }

}
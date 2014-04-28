using System;
using System.Collections.Generic;

using Castle.Core.Interceptor;
using Castle.DynamicProxy;

using HtmlElements.Elements;
using HtmlElements.Locators;
using HtmlElements.Proxy;

using OpenQA.Selenium;

using HtmlElements.Extensions;

namespace HtmlElements {

    public static class ElementFactory {

        private static readonly ProxyGenerator ProxyFactory = new ProxyGenerator();

        public static IList<T> CreateElementList<T>(IElementLocator locator, bool cache = false) where T : class, IWebElement {
            return Create(typeof(IList<T>), locator, cache) as IList<T>;
        }

        public static T CreateElement<T>(IElementLocator locator, bool cache = true) where T : class, IWebElement {
            return Create(typeof(T), locator, cache) as T;
        }

        public static Object Create(Type type, IElementLocator locator, bool cache = false) {
            if (type.IsWebElement()) return NewElement(type, locator, cache);
            if (type.IsWebElementList()) return NewElementList(type.GetGenericArguments()[0], locator, cache);

            throw new InvalidOperationException(String.Format("Cannot create instance of [{0}]", type));
        }

        private static object NewElement(Type type, IElementLocator locator, bool cache) {
            var proxy = GenerateProxy(typeof(IHtmlElement), new ElementProxy(locator, cache)) as IHtmlElement;

            return type == typeof(IHtmlElement) || type == typeof(IWebElement)
                ? new HtmlElement(proxy) : ObjectFactory.Create(type, proxy);
        }

        private static object NewElementList(Type type, IElementLocator locator, bool cache) {
            return GenerateProxy(typeof(IList<>).MakeGenericType(type), new ElementListProxy(type, locator, cache));
        }

        private static object GenerateProxy(Type @interface, IInterceptor interceptor) {
            return ProxyFactory.CreateInterfaceProxyWithoutTarget(@interface, interceptor);
        }

    }

}
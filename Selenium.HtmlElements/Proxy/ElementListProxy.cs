using System;
using System.Collections.Generic;

using Castle.Core.Interceptor;

using Selenium.HtmlElements.Factory;
using Selenium.HtmlElements.Locators;

namespace Selenium.HtmlElements.Proxy {

    internal class ElementListProxy : IInterceptor {

        private readonly IElementLocator _elementLocator;

        private readonly Type _elementType;

        public ElementListProxy(Type elementType, IElementLocator elementLocator) {
            _elementLocator = elementLocator;
            _elementType = elementType;
        }

        public void Intercept(IInvocation invocation) {
            invocation.ReturnValue = InvokeOnFoundElements(invocation);
        }

        private object InvokeOnFoundElements(IInvocation invocation) {
            var elementList = _elementLocator.FindElements();

            var proxyList = new List<Object>();

            for (var index = 0; index < elementList.Count; index++) {
                proxyList.Add(ElementFactory.Create(_elementType, new ListItemLocator(_elementLocator, index)));
            }

            return invocation.Method.Invoke(proxyList.AsReadOnly(), invocation.Arguments);
        }

    }

}
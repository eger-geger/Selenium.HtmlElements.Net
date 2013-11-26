using System;
using System.Collections.Generic;

using Castle.Core.Interceptor;

namespace Selenium.HtmlElements.Internal {

    internal class ElementListProxy : IInterceptor {

        private readonly IElementLocator _elementLocator;
        private readonly Type _elementType;
        private readonly bool _useCash;

        public ElementListProxy(Type elementType, IElementLocator elementLocator, bool useCash) {
            _elementLocator = elementLocator;
            _elementType = elementType;
            _useCash = useCash;
        }

        public void Intercept(IInvocation invocation) {
            invocation.ReturnValue = InvokeOnFoundElements(invocation);
        }

        private object InvokeOnFoundElements(IInvocation invocation) {
            var elementList = _elementLocator.FindElements();

            var proxyList = new List<Object>();

            for (var index = 0; index < elementList.Count; index++) {
                proxyList.Add(ElementFactory.Create(_elementType, new ListItemLocator(_elementLocator, index), _useCash));
            }

            return invocation.Method.Invoke(proxyList.AsReadOnly(), invocation.Arguments);
        }

    }

}
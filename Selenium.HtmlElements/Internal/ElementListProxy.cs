using System;
using System.Collections;
using System.Collections.Generic;

using Castle.Core.Interceptor;

namespace Selenium.HtmlElements.Internal {

    internal class ElementListProxy : IInterceptor {

        private readonly ElementLoader _elementLoader;
        private readonly Type _elementType;

        public ElementListProxy(Type elementType, IElementLocator elementLocator, bool useCach) {
            _elementLoader = new ElementLoader(elementLocator, useCach);
            _elementType = elementType;
        }

        public void Intercept(IInvocation invocation) {
            invocation.ReturnValue = InvokeOnElements(invocation);
        }

        private object InvokeOnElements(IInvocation invocation) {
            var loadedElements = _elementLoader.Load().WrappedElementList;

            var proxyList = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(_elementType));

            for (var index = 0; index < loadedElements.Count; index++) {
                proxyList.Add(ElementFactory.Create(_elementType, new ListItemLocator(_elementLoader.Locator, index),
                    _elementLoader.UseCach));
            }

            return invocation.Method.Invoke(proxyList, invocation.Arguments);
        }

    }

}
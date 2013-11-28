using System;
using System.Collections;
using System.Collections.Generic;

using Castle.Core.Interceptor;

namespace Selenium.HtmlElements.Internal {

    internal class ElementListProxy : IInterceptor {

        private readonly IList _elementList;
        private readonly ElementLoader _elementLoader;
        private readonly Type _elementType;

        public ElementListProxy(Type elementType, IElementLocator elementLocator, bool useCach) {
            _elementList = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            _elementLoader = new ElementLoader(elementLocator, useCach);
            _elementType = elementType;
        }

        public void Intercept(IInvocation invocation) {
            invocation.ReturnValue = InvokeOnElements(invocation);
            if (!_elementLoader.UseCach) _elementList.Clear();
        }

        private object InvokeOnElements(IInvocation invocation) {
            if (_elementList.Count == 0) {
                foreach (var element in _elementLoader.Load().WrappedElementList) {
                    _elementList.Add(ElementFactory.Create(_elementType, new SelfLocator(element),
                        _elementLoader.UseCach));
                }
            }

            return invocation.Method.Invoke(_elementList, invocation.Arguments);
        }

    }

}
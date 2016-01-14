using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Castle.DynamicProxy;
using HtmlElements.Locators;

namespace HtmlElements.Proxy {

    internal class ElementListProxy : IInterceptor {

        private readonly Type _listGenericType;
        private readonly ElementListLoader _loader;

        public ElementListProxy(Type type, IElementLocator locator, bool cache) {
            _loader = new ElementListLoader(locator, cache);
            _listGenericType = type;
        }

        private IList TypedElementList {
            get {
                var typedList = CreateTypedElementList();

                for (var i = 0; i < _loader.Load().Count; i++) {
                    typedList.Add(ElementFactory.Create(_listGenericType, new ListElementLocator(_loader.Load, i)));
                }

                return typedList;
            }
        }

        public void Intercept(IInvocation invocation) {
            invocation.ReturnValue = InvokeOnElements(TypedElementList, invocation);
        }

        private object InvokeOnElements(IList elementList, IInvocation invocation) {
            try {
                return invocation.Method.Invoke(elementList, invocation.Arguments);
            } catch (TargetInvocationException ex) {
                throw ex.InnerException;
            }
        }

        private IList CreateTypedElementList() {
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(_listGenericType)) as IList;
        }

    }

}
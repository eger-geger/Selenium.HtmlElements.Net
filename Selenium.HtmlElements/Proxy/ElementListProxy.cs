using System;

using Castle.Core.Interceptor;

using Selenium.HtmlElements.Locators;

namespace Selenium.HtmlElements.Proxy {

    internal class ElementListProxy : IInterceptor {

        private readonly ElementListLoader _loader;

        private readonly bool _useCache;

        public ElementListProxy(Type type, IElementLocator locator, bool useCache) {
            _useCache = useCache;
            _loader = new ElementListLoader(type, locator);
        } 

        public void Intercept(IInvocation invocation) {
            invocation.ReturnValue = InvokeOnElements(invocation);
        }

        private object InvokeOnElements(IInvocation invocation) {
            return invocation.Method.Invoke(_loader.Load(_useCache), invocation.Arguments);
        }

    }

}
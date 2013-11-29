using Castle.Core.Interceptor;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

using Selenium.HtmlElements.Extensions;
using Selenium.HtmlElements.Locators;

namespace Selenium.HtmlElements.Proxy {

    internal class ElementProxy : IInterceptor {

        private readonly ElementLoader _loader;

        private readonly bool _useCache;

        public ElementProxy(IElementLocator locator, bool useCache) {
            _loader = new ElementLoader(locator);
            _useCache = useCache;
        }

        public void Intercept(IInvocation invocation) {
            var loaded = _loader.Load(_useCache);

            if (invocation.Method.DeclaringType == typeof(IWrapsElement))
                invocation.ReturnValue = loaded;
            else if (invocation.Method.DeclaringType == typeof(IWrapsDriver))
                invocation.ReturnValue = loaded.ToWebDriver();
            else if (invocation.Method.DeclaringType == typeof(IJavaScriptExecutor))
                invocation.ReturnValue = InvokeOn(loaded.ToJavaScriptExecutor(), invocation);
            else invocation.ReturnValue = InvokeOn(loaded, invocation);
        }

        private object InvokeOn(object target, IInvocation invocation) {
            return invocation.Method.Invoke(target, invocation.Arguments);
        }

    }

}
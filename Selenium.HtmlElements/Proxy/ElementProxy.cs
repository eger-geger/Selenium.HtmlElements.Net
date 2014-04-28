using System.Reflection;

using Castle.Core.Interceptor;

using HtmlElements.Locators;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

using HtmlElements.Extensions;

namespace HtmlElements.Proxy {

    internal class ElementProxy : IInterceptor {

        private readonly ElementLoader _loader;

        public ElementProxy(IElementLocator locator, bool cache) {
            _loader = new ElementLoader(locator, cache);
        }

        public void Intercept(IInvocation invocation) {
            var loaded = _loader.Load();

            if (invocation.Method.DeclaringType == typeof(IWrapsElement)) {
                invocation.ReturnValue = loaded;
            } else if (invocation.Method.DeclaringType == typeof(IWrapsDriver)) {
                invocation.ReturnValue = loaded.ToWebDriver();
            } else if (invocation.Method.DeclaringType == typeof(IJavaScriptExecutor)) {
                invocation.ReturnValue = InvokeUnwrappingExceptions(loaded.ToJavaScriptExecutor(), invocation);
            } else {
                invocation.ReturnValue = InvokeUnwrappingExceptions(loaded, invocation);
            }
        }

        private static object InvokeUnwrappingExceptions(object target, IInvocation invocation) {
            try {
                return invocation.Method.Invoke(target, invocation.Arguments);
            } catch (TargetInvocationException ex) {
                throw ex.InnerException;
            }
        }

    }

}
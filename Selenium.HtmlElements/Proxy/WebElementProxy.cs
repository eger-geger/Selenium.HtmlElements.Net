using Castle.Core.Interceptor;

using OpenQA.Selenium.Internal;

using Selenium.HtmlElements.Locators;

namespace Selenium.HtmlElements.Proxy {

    internal class WebElementProxy : IInterceptor {

        private readonly WebElementCash _cash;

        public WebElementProxy(IElementLocator locator) {
            _cash = new WebElementCash(locator) {
                UnableToLoadMessage = string.Format("Failed to load element with locator {0}", locator)
            };
        }

        public void Intercept(IInvocation invocation) {
            if (invocation.Method.DeclaringType == typeof(IWrapsElement))
                invocation.ReturnValue = _cash.Load().WrappedElement;
            else if (invocation.Method.DeclaringType == typeof(IWrapsDriver))
                invocation.ReturnValue = _cash.Load().WrappedDriver;
            else invocation.ReturnValue = InvokeOnCashedElement(invocation);
        }

        private object InvokeOnCashedElement(IInvocation invocation) {
            return invocation.Method.Invoke(_cash.Load().WrappedElement, invocation.Arguments);
        }

    }

}
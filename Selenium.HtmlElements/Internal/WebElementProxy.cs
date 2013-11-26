using Castle.Core.Interceptor;

using OpenQA.Selenium.Internal;

namespace Selenium.HtmlElements.Internal {

    internal class WebElementProxy : IInterceptor {

        private readonly ElementLoader _loader;

        public WebElementProxy(IElementLocator locator, bool useCash) {
            _loader = new ElementLoader(locator, useCash);
        }

        public void Intercept(IInvocation invocation) {
            if (invocation.Method.DeclaringType == typeof(IWrapsElement)) 
                invocation.ReturnValue = _loader.Load().WrappedElement;
            else if (invocation.Method.DeclaringType == typeof(IWrapsDriver)) 
                invocation.ReturnValue = _loader.Load().WrappedDriver;
            else invocation.ReturnValue = InvokeOnLoadedElement(invocation);
        }

        private object InvokeOnLoadedElement(IInvocation invocation) {
            return invocation.Method.Invoke(_loader.Load().WrappedElement, invocation.Arguments);
        }

    }

}
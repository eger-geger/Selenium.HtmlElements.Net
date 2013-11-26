using System;

using Castle.Core.Interceptor;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Selenium.HtmlElements.Internal {

    internal class WebElementProxy : IInterceptor {

        private readonly Func<IWebElement> _loadElement;

        public WebElementProxy(IElementLocator locator, bool useCash) {
            if (useCash) {
                var cash = new WebElementCash(locator) {
                    UnableToLoadMessage = string.Format("Failed to load element with locator {0}", locator)
                };

                _loadElement = () => UnwrapElement(cash.Load().WrappedElement);
            }

            _loadElement = () => UnwrapElement(locator.FindElement());
        }

        public void Intercept(IInvocation invocation) {
            if (invocation.Method.DeclaringType == typeof(IWrapsElement)) invocation.ReturnValue = _loadElement();
            else if (invocation.Method.DeclaringType == typeof(IWrapsDriver)) invocation.ReturnValue = UnwrapDriver(_loadElement());
            else invocation.ReturnValue = InvokeOnLoadedElement(invocation);
        }

        private object InvokeOnLoadedElement(IInvocation invocation) {
            return invocation.Method.Invoke(_loadElement(), invocation.Arguments);
        }

        private IWebElement UnwrapElement(IWebElement webElement) {
            var wrapper = webElement as IWrapsElement;

            if (wrapper == null) return webElement;

            return UnwrapElement(wrapper.WrappedElement);
        }

        private IWebDriver UnwrapDriver(IWebElement webElement) {
            var driverWrapper = webElement as IWrapsDriver;

            if (driverWrapper != null) return driverWrapper.WrappedDriver;

            throw new ArgumentException("Not a driver wrapper", "webElement");
        }

    }

}
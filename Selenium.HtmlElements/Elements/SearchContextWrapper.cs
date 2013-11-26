using System;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

using Selenium.HtmlElements.Internal;

namespace Selenium.HtmlElements.Elements {

    public abstract class SearchContextWrapper : ISearchContext, IWrapsDriver, IJavaScriptExecutor {

        private readonly ISearchContext _wrappedContext;

        protected SearchContextWrapper(ISearchContext wrapped) {
            _wrappedContext = (wrapped is SearchContextWrapper)
                ? (wrapped as SearchContextWrapper)._wrappedContext
                : wrapped;
        }

        public object ExecuteScript(string script, params object[] args) {
            var jsExecutor = WrappedDriver as IJavaScriptExecutor;

            if (jsExecutor != null) return jsExecutor.ExecuteScript(script, args);

            throw new InvalidOperationException("Underlying WebDriver cannot execute javascript");
        }

        public object ExecuteAsyncScript(string script, params object[] args) {
            var jsExecutor = WrappedDriver as IJavaScriptExecutor;

            if (jsExecutor != null) return jsExecutor.ExecuteAsyncScript(script, args);

            throw new InvalidOperationException("Underlying WebDriver cannot execute javascript");
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by) {
            return _wrappedContext.FindElements(@by);
        }

        public IWebElement FindElement(By @by) {
            return _wrappedContext.FindElement(@by);
        }

        public IWebDriver WrappedDriver {
            get {
                if (_wrappedContext is IWebDriver) return _wrappedContext as IWebDriver;
                if (_wrappedContext is IWrapsDriver) return (_wrappedContext as IWrapsDriver).WrappedDriver;

                throw new InvalidOperationException("Does not wrapp IWebDriver");
            }
        }

        protected IElementLocator RelativeLocator(By by) {
            return new ElementLocator(this, @by);
        }

        public override string ToString() {
            return _wrappedContext.ToString();
        }

    }

}
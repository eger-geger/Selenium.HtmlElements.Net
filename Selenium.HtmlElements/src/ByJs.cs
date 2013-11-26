using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Selenium.HtmlElements {

    public class ByJs : By {

        public ByJs(string locator) {
            FindElementMethod = delegate(ISearchContext context) {
                var result = FindByJs(context, locator);

                if (result is IList<IWebElement>) return (result as IList<IWebElement>)[0];

                if (result is IWebElement) return result as IWebElement;

                throw new NoSuchElementException(string.Format("Failed to find by {0}", this));
            };

            FindElementsMethod = delegate(ISearchContext context) {
                var result = FindByJs(context, locator);

                if (result is IList<IWebElement>) return new List<IWebElement>(result as IList<IWebElement>).AsReadOnly();

                if (result is IWebElement) return new List<IWebElement> {result as IWebElement}.AsReadOnly();

                throw new NoSuchElementException(string.Format("Failed to find by {0}", this));
            };

            Description = string.Format("By.JavaScript: {0}", locator);
        }

        private static Object FindByJs(ISearchContext context, string jsLocator) {
            var jsExecutor = ExtractJsExecutorFrom(context);

            if (jsExecutor == null) throw new InvalidOperationException(string.Format("Cannot search in {0} with javascript", context));

            try {
                return jsExecutor.ExecuteScript(string.Format("return {0} ;", jsLocator));
            } catch (Exception) {
                throw new NoSuchElementException(string.Format("Element {0} not found within {1}", jsLocator, context));
            }
        }

        private static IJavaScriptExecutor ExtractJsExecutorFrom(ISearchContext context) {
            if (context is IJavaScriptExecutor) return context as IJavaScriptExecutor;
            if (context is IWrapsDriver) return ExtractJsExecutorFrom((context as IWrapsDriver).WrappedDriver);

            return null;
        }

    }

}
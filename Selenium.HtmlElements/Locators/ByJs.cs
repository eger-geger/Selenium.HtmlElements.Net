using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Selenium.HtmlElements.Locators {

    public class ByJs : By {

        protected ByJs(string jsLocator) {
            FindElementMethod = context => FindByJs(context, jsLocator) as IWebElement;

            FindElementsMethod = delegate(ISearchContext context) {
                var elementList = FindByJs(context, jsLocator) as IList<IWebElement>;

                return elementList == null
                    ? new List<IWebElement>().AsReadOnly()
                    : new List<IWebElement>(elementList).AsReadOnly();
            };
        }

        private static Object FindByJs(ISearchContext context, string jsLocator) {
            var jsExecutor = ExtractJsExecutorFrom(context);

            if (jsExecutor == null)
                throw new InvalidOperationException(string.Format("Cannot search in {0} with javascript", context));

            return jsExecutor.ExecuteScript(string.Format("return {0} ;", jsLocator));
        }

        private static IJavaScriptExecutor ExtractJsExecutorFrom(ISearchContext context) {
            if (context is IJavaScriptExecutor)
                return context as IJavaScriptExecutor;
            if (context is IWrapsDriver)
                return ExtractJsExecutorFrom((context as IWrapsDriver).WrappedDriver);

            return null;
        }

    }

}
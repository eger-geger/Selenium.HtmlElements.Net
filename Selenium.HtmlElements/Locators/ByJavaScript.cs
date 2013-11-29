using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using OpenQA.Selenium;

using HtmlElements.Extensions;

namespace HtmlElements {

    public class ByJavaScript : By {

        public ByJavaScript(string locator) {
            FindElementMethod = context => ToWebElement(FindByJs(context, locator));
            FindElementsMethod = context => ToElementList(FindByJs(context, locator));

            Description = string.Format("By.JavaScript: {0}", locator);
        }

        private static Object FindByJs(ISearchContext context, string jsLocator) {
            var jsExecutor = context.ToJavaScriptExecutor();

            if (jsExecutor == null)
                throw new ArgumentException(string.Format("[{0}] cannot execute JavaScript", context), "context");

            try {
                return jsExecutor.ExecuteScript(string.Format("return {0} ;", jsLocator));
            } catch (Exception ex) {
                throw new ArgumentException(string.Format("Wrong locator [{0}]", jsLocator), "jsLocator", ex);
            }
        }

        private IWebElement ToWebElement(object searchResult) {
            if (searchResult is IList<IWebElement>) return (searchResult as IList<IWebElement>)[0];
            if (searchResult is IWebElement) return searchResult as IWebElement;

            throw new NoSuchElementException(string.Format("Element not found [{0}]", this));
        }

        private ReadOnlyCollection<IWebElement> ToElementList(object searchResult) {
            if (searchResult is IList<IWebElement>)
                return new List<IWebElement>(searchResult as IList<IWebElement>).AsReadOnly();
            if (searchResult is IWebElement) return new List<IWebElement> {searchResult as IWebElement}.AsReadOnly();

            return Enumerable.Empty<IWebElement>().ToList().AsReadOnly();
        }

    }

}
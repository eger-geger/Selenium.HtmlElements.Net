using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Locators
{
    /// <summary>
    ///     Use JavaScript code to locate WebElement
    /// </summary>
    public class ByJavaScript : By
    {
        private readonly Object[] _arguments;
        private readonly String _javaScript;

        /// <summary>
        ///     Creates new instance of JavaScript element finder.
        /// </summary>
        /// <param name="javaScript">JavaScript code which returns collection of element or single element</param>
        /// <param name="arguments">Arguments passed to <see cref="IJavaScriptExecutor"/> when locating element</param>
        public ByJavaScript(String javaScript, params Object[] arguments)
        {
            if (String.IsNullOrEmpty(javaScript))
            {
                throw new ArgumentNullException("javaScript", "JavaScript snippet is null or empty");
            }

            _javaScript = String.Format("return {0};", javaScript);
            _arguments = arguments;

            FindElementMethod = FindElementWithJavaScript;
            FindElementsMethod = FindElementListWithJavaScript;
            Description = String.Format("By.JavaScript: {0}, {1}", javaScript, arguments.ToCommaSeparatedString());
        }

        private IWebElement FindElementWithJavaScript(ISearchContext context)
        {
            return ToWebElement(FindWithJavaScript(context));
        }

        private ReadOnlyCollection<IWebElement> FindElementListWithJavaScript(ISearchContext searchContext)
        {
            return ToElementList(FindWithJavaScript(searchContext));
        }

        private Object FindWithJavaScript(ISearchContext searchContext)
        {
            var jsExecutor = searchContext.ToJavaScriptExecutor();

            if (jsExecutor == null)
            {
                throw new ArgumentException("Wrapped WebDriver cannot execute JavaScript", "searchContext");
            }

            try
            {
                return jsExecutor.ExecuteScript(_javaScript, _arguments);
            }
            catch (Exception ex)
            {
                throw new NoSuchElementException(
                    String.Format("Failed to find element {0}", Description), ex
                );
            }
        }

        private IWebElement ToWebElement(object searchResult)
        {
            if (searchResult is IList<IWebElement>)
            {
                return (searchResult as IList<IWebElement>)[0];
            }

            if (searchResult is IWebElement)
            {
                return searchResult as IWebElement;
            }

            throw new NoSuchElementException(String.Format("Element not found [{0}]", this));
        }

        private static ReadOnlyCollection<IWebElement> ToElementList(object searchResult)
        {
            if (searchResult is IList<IWebElement>)
            {
                return new List<IWebElement>((IList<IWebElement>) searchResult).AsReadOnly();
            }

            if (searchResult is IWebElement)
            {
                return new List<IWebElement> {(IWebElement) searchResult}.AsReadOnly();
            }

            return Enumerable.Empty<IWebElement>().ToList().AsReadOnly();
        }
    }
}
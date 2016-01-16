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
                throw new ArgumentNullException(nameof(javaScript), "JavaScript snippet is null or empty");
            }

            _javaScript = javaScript;
            _arguments = arguments;

            FindElementMethod = FindElementWithJavaScript;
            FindElementsMethod = FindElementListWithJavaScript;
            Description = $"By.JavaScript: {javaScript}, {arguments.ToCommaSepratedString()}";
        }

        private IWebElement FindElementWithJavaScript(ISearchContext context)
        {
            return ToWebElement(FindWithJavaScript(context));
        }

        private ReadOnlyCollection<IWebElement> FindElementListWithJavaScript(ISearchContext searchContext)
        {
            return ToElementList(FindElementWithJavaScript(searchContext));
        }

        private Object FindWithJavaScript(ISearchContext searchContext)
        {
            var jsExecutor = searchContext.ToJavaScriptExecutor();

            if (jsExecutor == null)
            {
                throw new ArgumentException("Wrapped WebDriver cannot execute JavaScript", nameof(searchContext));
            }

            try
            {
                return jsExecutor.ExecuteScript(_javaScript, _arguments);
            }
            catch (Exception ex)
            {
                throw new NoSuchElementException($"Failed to find element {Description}", ex);
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

            throw new NoSuchElementException($"Element not found [{this}]");
        }

        private ReadOnlyCollection<IWebElement> ToElementList(object searchResult)
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
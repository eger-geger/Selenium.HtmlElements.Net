using System;
using System.Linq;

using HtmlElements.Elements;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Extensions {

    /// <summary>
    ///     Collection of extension methods for web elements and web pages
    /// </summary>
    public static class WebElementExtensions {

        /// <summary> 
        ///     Checks weather text is shown on page.
        /// </summary>
        /// <param name="page">HTML page to search text on</param>
        /// <param name="text">List of string to check</param>
        /// <returns><c>true</c> if every string from the list is shown on a page and <c>false</c> otherwise</returns>
        public static bool IsTextShown(this HtmlPage page, params String[] text) {
            var bodyText = page.Body.Text;

            return text.All(bodyText.Contains);
        }

        /// <summary> 
        ///     Check weather text exist on page.
        /// </summary>
        /// <param name="page">HTML page to search text on</param>
        /// <param name="text">List of string to check</param>
        /// <returns><c>true</c> if every string from the list exist on a page and <c>false</c> otherwise</returns>
        public static bool IsTextPresent(this HtmlPage page, params String[] text)
        {
            var pageSource = page.Source;

            return text.All(pageSource.Contains);
        }

        /// <summary> 
        ///     Check if all text chunks are not shown on page 
        /// </summary>
        /// <param name="text">text to check</param>
        /// <returns>
        ///     true if all provided chunks are not present in page source and false otherwise
        /// </returns>
        public static bool IsTextNotPresent(this HtmlPage page, params String[] text) {
            return text.All(t => !page.Source.Contains(t));
        }

        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        private static TTarget IsVisible<TTarget>(this TTarget element) where TTarget : class, IWebElement {
            return IsPresent(element as IWebElement) && element.Displayed ? element : null;
        }

        /// <summary>
        ///     Checks weather element exists in DOM.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        private static TTarget IsPresent<TTarget>(this TTarget element) where TTarget : class, IWebElement {
            return IsPresent(element as IWebElement) ? element : null;
        }

        /// <summary>
        ///     Checks weather element exists in DOM.
        /// </summary>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        public static bool IsPresent(this IWebElement element) {
            try {
                var ignore = element.Size;
            } catch (WebDriverException) {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Checks weather element is hidden.
        /// </summary>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> if element is hidden or does not exist and <c>false</c> otherwise</returns>
        public static bool IsHidden(this IWebElement element) {
            return !IsPresent(element) && !element.Displayed;
        }

        public static T As<T>(this IWebElement self) where T : class {
            var webElement = self is IWrapsElement ? (self as IWrapsElement).WrappedElement : self;

            return ObjectFactory.CreatePageObject(typeof(T), webElement) as T;
        }

        /// <summary>
        ///     Wait until element became hidden for 10 seconds checking it every second
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="target">Target element</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="pollingInterval">Determines how often command will be evaluated until it expires or succeeds</param>
        /// <param name="message">Error message used when command expires</param>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not hide after 10 seconds
        /// </exception>
        public static void WaitUntilHidden<TTarget>(this TTarget target, TimeSpan commandTimeout, TimeSpan pollingInterval, String message = null) where TTarget : class, IWebElement {
            try
            {
                target.WaitUntil(IsHidden, commandTimeout, pollingInterval, message ?? String.Format("{0} did not became hidden after {1}", target, commandTimeout));
            }
            catch (NoSuchElementException)
            {
                //Element removed from DOM
            }
            catch (StaleElementReferenceException)
            {
                //Element removed from DOM
            }
        }

        /// <summary>
        ///     Wait until element became hidden for 10 seconds checking it every second
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="target">Target element</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="message">Error message used when command expires</param>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not hide after 10 seconds
        /// </exception>
        public static void WaitUntilHidden<TTarget>(this TTarget target, TimeSpan commandTimeout, String message = null) where TTarget : class, IWebElement {
            try
            {
                target.WaitUntil(IsHidden, commandTimeout, message ?? String.Format("{0} did not became hidden after {1}", target, commandTimeout));
            }
            catch (NoSuchElementException)
            {
                //Element removed from DOM
            }
            catch (StaleElementReferenceException)
            {
                //Element removed from DOM
            }
        }

        /// <summary>
        ///     Wait until element became hidden for 10 seconds checking it every second
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="target">Target element</param>
        /// <param name="message">Error message used when command expires</param>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not hide after 10 seconds
        /// </exception>
        public static void WaitUntilHidden<TTarget>(this TTarget target, String message = null) where TTarget : class, IWebElement {
            try
            {
                target.WaitUntil(IsHidden, message ?? String.Format("{0} did not became hidden after 10 seconds", target));
            }
            catch (NoSuchElementException)
            {
                //Element removed from DOM
            }
            catch (StaleElementReferenceException)
            {
                //Element removed from DOM
            }
        }

        /// <summary>
        ///     Wait until element became present on page (get created in DOM) and return the element itself.
        ///     Current overload waits for 10 seconds and checks weather element is present every second.
        /// </summary>
        /// <typeparam name="TTarget">Type of th target element</typeparam>
        /// <param name="target">Element expected to be created in DOM</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Element once it became visible</returns>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not appear in DOM after 10 seconds
        /// </exception>
        public static TTarget WaitForPresent<TTarget>(this TTarget target, String message = null) where TTarget : class, IWebElement {
            return target.WaitFor(IsPresent, message ?? String.Format("{0} did not appear in DOM after 10 seconds", target));
        }

        /// <summary>
        ///     Wait until element became present on page (get created in DOM) and return the element itself.
        ///     Current overload waits for a given timeout and checks weather element is present every second.
        /// </summary>
        /// <typeparam name="TTarget">Type of th target element</typeparam>
        /// <param name="target">Element expected to be created in DOM</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Element once it became visible</returns>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not appear in DOM after 10 seconds
        /// </exception>
        public static TTarget WaitForPresent<TTarget>(this TTarget target, TimeSpan commandTimeout, String message = null) where TTarget : class, IWebElement {
            return target.WaitFor(IsPresent, commandTimeout, message ?? String.Format("{0} did not appear in DOM after {1}", target, commandTimeout));
        }

        /// <summary>
        ///     Wait until element became present on page (get created in DOM) and return the element itself.
        /// </summary>
        /// <typeparam name="TTarget">Type of th target element</typeparam>
        /// <param name="target">Element expected to be created in DOM</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="pollingInterval">Determines how often command will be evaluated until it expires or succeeds</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Element once it became visible</returns>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not appear in DOM after 10 seconds
        /// </exception>
        public static TTarget WaitForPresent<TTarget>(this TTarget target, TimeSpan commandTimeout, TimeSpan pollingInterval, String message = null) where TTarget : class, IWebElement {
            return target.WaitFor(IsPresent, commandTimeout, pollingInterval, message ?? String.Format("{0} did not appear in DOM after {1}", target, commandTimeout));
        }

        /// <summary>
        ///     Wait until element is being displayed on a page.
        ///     Current overload waits for 10 seconds checking element visibility every second.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="target">Target element</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Target element once it became displayed on a page</returns>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not became visible after 10 seconds
        /// </exception>
        public static TTarget WaitForVisible<TTarget>(this TTarget target, String message = null) where TTarget : class, IWebElement {
            return target.WaitFor(IsVisible, message ?? String.Format("{0} did not became visible after 10 seconds", target));
        }

        /// <summary>
        ///     Wait until element is being displayed on a page.
        ///     Current overload waits for a given time checking element visibility every second.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="target">Target element</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Target element once it became displayed on a page</returns>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not became visible after a given timeout
        /// </exception>
        public static TTarget WaitForVisible<TTarget>(this TTarget target, TimeSpan commandTimeout, String message = null) where TTarget : class, IWebElement {
            return target.WaitFor(IsVisible, commandTimeout, message ?? String.Format("{0} did not became visible after {1}", target, commandTimeout));
        }

        /// <summary>
        ///     Wait until element is being displayed on a page.
        ///     Current overload waits for a given time checking element visibility with provided polling interval.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="target">Target element</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="pollingInterval">Determines how often command will be evaluated until it expires or succeeds</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Target element once it became displayed on a page</returns>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown when element did not became visible after a given timeout
        /// </exception>
        public static TTarget WaitForVisible<TTarget>(this TTarget target, TimeSpan commandTimeout, TimeSpan pollingInterval, String message = null) where TTarget : class, IWebElement {
            return target.WaitFor(IsVisible, commandTimeout, pollingInterval, message ?? String.Format("{0} did not became visible after {1}", target, commandTimeout));
        }

    }

}
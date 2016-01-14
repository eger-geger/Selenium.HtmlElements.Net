using System;
using System.Linq;

using HtmlElements.Elements;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Extensions {

    public static class WebElementExtensions {

        /// <summary> Check if all text chunks are shown on page </summary>
        /// <param name="text">text to check</param>
        /// <returns>
        ///     true if all provided chunk are shown on page and false otherwise
        /// </returns>
        public static bool IsTextShown(this HtmlPage page, params String[] text) {
            var bodyText = page.Body.Text;

            return text.All(bodyText.Contains);
        }

        /// <summary> Check if all text chunks are shown on page </summary>
        /// <param name="text">text to check</param>
        /// <returns>
        ///     true if all provided chunks are present in page source and false otherwise
        /// </returns>
        public static bool IsTextPresent(this HtmlPage page, params String[] text) {
            return text.All(page.Source.Contains);
        }

        /// <summary> Check if all text chunks are not shown on page </summary>
        /// <param name="text">text to check</param>
        /// <returns>
        ///     true if all provided chunks are not present in page source and false otherwise
        /// </returns>
        public static bool IsTextNotPresent(this HtmlPage page, params String[] text) {
            return text.All(t => !page.Source.Contains(t));
        }

        private static T IsVisible<T>(this T element) where T : class, IWebElement {
            return IsPresent(element as IWebElement) && element.Displayed ? element : null;
        }

        private static T IsPresent<T>(this T element) where T : class, IWebElement {
            return IsPresent(element as IWebElement) ? element : null;
        }

        public static bool IsPresent(this IWebElement self) {
            try {
                var ignore = self.Size;
            } catch (WebDriverException) {
                return false;
            }

            return true;
        }

        public static bool IsHidden(this IWebElement element) {
            return !element.Displayed;
        }

        public static T As<T>(this IWebElement self) where T : class {
            var webElement = self is IWrapsElement ? (self as IWrapsElement).WrappedElement : self;

            return ObjectFactory.CreatePageObject(typeof(T), webElement) as T;
        }

        public static void WaitUntilHidden<T>(this T self, TimeSpan timeout, TimeSpan polling) where T : class, IWebElement {
            try
            {
                self.WaitUntil(IsHidden, timeout, polling);
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

        public static void WaitUntilHidden<T>(this T self, TimeSpan timeout) where T : class, IWebElement {
            try
            {
                self.WaitUntil(IsHidden, timeout);
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

        public static void WaitUntilHidden<T>(this T self) where T : class, IWebElement {
            try
            {
                self.WaitUntil(IsHidden);
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

        public static T WaitForVisible<T>(this T self) where T : class, IWebElement {
            return self.WaitFor(IsVisible);
        }

        public static T WaitForVisible<T>(this T self, TimeSpan timeout) where T : class, IWebElement {
            return self.WaitFor(IsVisible, timeout);
        }

        public static T WaitForVisible<T>(this T self, TimeSpan timeout, TimeSpan polling) where T : class, IWebElement {
            return self.WaitFor(IsVisible, timeout, polling);
        }

    }

}
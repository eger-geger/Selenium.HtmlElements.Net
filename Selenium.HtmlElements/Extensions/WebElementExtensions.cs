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

            return ObjectFactory.Create(typeof(T), webElement) as T;
        }

        public static void WaitUntilHidden<T>(this T self, TimeSpan timeout, TimeSpan polling)
            where T : class, IWebElement {
            self.WaitUntil(IsHidden, timeout, polling);
        }

        public static void WaitUntilHidden<T>(this T self, TimeSpan timeout) where T : class, IWebElement {
            self.WaitUntil(IsHidden, timeout);
        }

        public static void WaitUntilHidden<T>(this T self) where T : class, IWebElement {
            self.WaitUntil(IsHidden);
        }

        public static T WaitForPresent<T>(this T self) where T : class, IWebElement {
            return self.WaitFor(IsPresent);
        }

        public static T WaitForPresent<T>(this T self, TimeSpan timeout) where T : class, IWebElement {
            return self.WaitFor(IsPresent, timeout);
        }

        public static T WaitForPresent<T>(this T self, TimeSpan timeout, TimeSpan polling) where T : class, IWebElement {
            return self.WaitFor(IsPresent, timeout, polling);
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
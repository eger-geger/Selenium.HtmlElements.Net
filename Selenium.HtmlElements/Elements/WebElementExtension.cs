using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

using Selenium.HtmlElements.Conditional;

namespace Selenium.HtmlElements.Elements {

    public static class WebElementExtension {

        private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMilliseconds(500);

        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

        public static bool IsPresent(this IWebElement self) {
            try {
                var ignore = self.Size;
            } catch (NoSuchElementException) {
                return false;
            }

            return true;
        }

        public static TElement WaitForPresent<TElement>(this TElement self) where TElement : class, IWebElement {
            return WaitForPresent(self, DefaultTimeout);
        }

        public static TElement WaitForPresent<TElement>(this TElement self, TimeSpan timeout)
            where TElement : class, IWebElement {
            return WaitForPresent(self, timeout, DefaultPollingInterval);
        }

        public static TElement WaitForPresent<TElement>(this TElement self, TimeSpan timeout, TimeSpan pollingInterval)
            where TElement : class, IWebElement {
            return WaitForResult(self, element => IsPresent(element) ? element : null, timeout, pollingInterval);
        }

        public static TElement WaitForVisible<TElement>(this TElement self) where TElement : class, IWebElement {
            return WaitForVisible(self, DefaultTimeout);
        }

        public static TElement WaitForVisible<TElement>(this TElement self, TimeSpan timeout)
            where TElement : class, IWebElement {
            return WaitForVisible(self, timeout, DefaultPollingInterval);
        }

        public static TElement WaitForVisible<TElement>(this TElement self, TimeSpan timeout, TimeSpan pollingInterval)
            where TElement : class, IWebElement {
            return WaitForResult(self, element => element.Displayed ? element : null, timeout, pollingInterval);
        }

        public static TResult WaitForResult<TElement, TResult>(this TElement self, Func<TElement, TResult> condition)
            where TElement : class, IWebElement {
            return WaitForResult(self, condition, DefaultTimeout);
        }

        public static TResult WaitForResult<TElement, TResult>(this TElement self, Func<TElement, TResult> condition,
                                                               TimeSpan timeout) where TElement : class, IWebElement {
            return WaitForResult(self, condition, timeout, DefaultPollingInterval);
        }

        public static TResult WaitForResult<TElement, TResult>(this TElement self, Func<TElement, TResult> condition,
                                                               TimeSpan timeout, TimeSpan pollingInterval)
            where TElement : class, IWebElement {
            var wait = new DefaultWait<TElement>(self) {
                Message = string.Format("{0} expires after {1}", condition, timeout),
                PollingInterval = pollingInterval,
                Timeout = timeout
            };

            wait.IgnoreExceptionTypes(typeof(WebDriverException));

            return wait.Until(condition);
        }

        public static void WaitForState<TElement>(this TElement self, Predicate<TElement> condition)
            where TElement : class, IWebElement {
            WaitForState(self, condition, DefaultTimeout);
        }

        public static void WaitForState<TElement>(this TElement self, Predicate<TElement> condition, TimeSpan timeout)
            where TElement : class, IWebElement {
            WaitForState(self, condition, timeout, DefaultPollingInterval);
        }

        public static void WaitForState<TElement>(this TElement self, Predicate<TElement> condition, TimeSpan timeout,
                                                  TimeSpan pollingInterval) where TElement : class, IWebElement {
            WaitForResult(self, condition.Invoke, timeout, pollingInterval);
        }

        public static TReturn As<TReturn>(this IWebElement self) where TReturn : class, new() {
            var webElement = self is IWrapsElement ? (self as IWrapsElement).WrappedElement : self;

            return Activator.CreateInstance(typeof(TReturn), webElement) as TReturn;
        }
    
        public static ConditionalActionExecutor<TElement> Do<TElement>(this TElement self, Action<TElement> action) where TElement : class, IWebElement {
            return new ConditionalActionExecutor<TElement>(action);
        }

    }

}
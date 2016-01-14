using System;

using HtmlElements.Actions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace HtmlElements.Extensions {

    public static class ActionExtension {

        private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMilliseconds(500);

        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

        public static ConditionalActionExecutor<T> Do<T>(this T self, Action<T> action) where T : class {
            return new ConditionalActionExecutor<T>(action, self).Every(DefaultPollingInterval).For(DefaultTimeout);
        }

        public static ConditionalActionExecutor<T> Do<T>(this T self, Action action) where T : class {
            return Do(self, s => action());
        }

        public static TR WaitFor<TE, TR>(this TE self, Func<TE, TR> condition) where TE : class {
            return WaitFor(self, condition, DefaultTimeout);
        }

        public static TR WaitFor<TE, TR>(this TE self, Func<TE, TR> condition, TimeSpan timeout) where TE : class {
            return WaitFor(self, condition, timeout, DefaultPollingInterval);
        }

        public static TR WaitFor<TE, TR>(this TE self, Func<TE, TR> condition, TimeSpan timeout, TimeSpan polling) where TE : class {
            var wait = new DefaultWait<TE>(self) {
                Message = String.Format("{0} expires after {1}", condition, timeout),
                PollingInterval = polling,
                Timeout = timeout
            };
            
            wait.IgnoreExceptionTypes(typeof(WebDriverException));

            return wait.Until(condition);
        }

        public static void WaitUntil<T>(this T self, Predicate<T> condition) where T : class {
            WaitUntil(self, condition, DefaultTimeout);
        }

        public static void WaitUntil<T>(this T self, Predicate<T> condition, TimeSpan timeout) where T : class {
            WaitUntil(self, condition, timeout, DefaultPollingInterval);
        }

        public static void WaitUntil<T>(this T self, Predicate<T> condition, TimeSpan timeout, TimeSpan polling) where T : class {
            WaitFor(self, condition.Invoke, timeout, polling);
        }

    }

}
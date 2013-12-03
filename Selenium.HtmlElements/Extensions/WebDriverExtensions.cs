using System;
using System.Linq;

using OpenQA.Selenium;

namespace HtmlElements.Extensions {

    public static class WebDriverExtensions {

        public static void SwitchToLastOpenedWindow(this IWebDriver self) {
            self.SwitchTo().Window(self.WindowHandles.Last());
        }

        public static void WaitUntilNewWindowOpened(this IWebDriver self, Action newWindowTrigger) {
            var initWindowCount = self.WindowHandles.Count;

            newWindowTrigger();

            self.WaitUntil(s => s.WindowHandles.Count > initWindowCount);
        }

        public static void WaitUntilNewWindowOpened(this IWebDriver self, Action newWindowTrigger, TimeSpan timeout) {
            var initWindowCount = self.WindowHandles.Count;

            newWindowTrigger();

            self.WaitUntil(s => s.WindowHandles.Count > initWindowCount, timeout);
        }

    }

}
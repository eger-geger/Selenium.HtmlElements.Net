using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Selenium.HtmlElements.Extensions {

    public static class SearchContextExtensions {

        public static IWebElement TryFindElement(this ISearchContext self, By by) {
            try {
                return self.FindElement(by);
            } catch (WebDriverException) {
                return null;
            }
        }

        public static IWebDriver ToWebDriver(this ISearchContext self) {
            if (self is IWebDriver) return self as IWebDriver;
            if (self is IWrapsDriver) return (self as IWrapsDriver).WrappedDriver;
            if (self is IWrapsElement) return (self as IWrapsElement).WrappedElement.ToWebDriver();

            return null;
        }

        public static IJavaScriptExecutor ToJavaScriptExecutor(this ISearchContext self) {
            if (self is IJavaScriptExecutor) return self as IJavaScriptExecutor;
            if (self is IWrapsDriver) return (self as IWrapsDriver).WrappedDriver.ToJavaScriptExecutor();
            if (self is IWrapsElement) return (self as IWrapsElement).WrappedElement.ToJavaScriptExecutor();

            return null;
        }

    }

}
using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

using Selenium.HtmlElements.Conditional;
using Selenium.HtmlElements.Internal;

namespace Selenium.HtmlElements.Elements {

    public static class WebElementExtension {

        public static bool IsPresent(this IWebElement self) {
            try {
                var ignore = self.Size;
            } catch (WebDriverException) {
                return false;
            }

            return true;
        }

        public static TReturn As<TReturn>(this IWebElement self) where TReturn : class {
            var webElement = self is IWrapsElement ? (self as IWrapsElement).WrappedElement : self;

            return PageObjectFactory.Create(typeof(TReturn), webElement) as TReturn;
        }

        public static ConditionalActionExecutor<TElement> Do<TElement>(this TElement self, Action<TElement> action)
            where TElement : class {
            return new ConditionalActionExecutor<TElement>(action).On(self);
        }

    }

}
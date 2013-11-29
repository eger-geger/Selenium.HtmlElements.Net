using System;
using System.Collections.Generic;

using OpenQA.Selenium;

using Selenium.HtmlElements.Extensions;
using Selenium.HtmlElements.Locators;

namespace Selenium.HtmlElements.Proxy {

    internal class ElementLoader : CachedElementLoader<IWebElement> {

        public ElementLoader(IElementLocator locator) : base(locator.FindElement, IsElementLoaded) {
            IgnoredExceptionTypes = new List<Type> {typeof(StaleElementReferenceException)};
        }

        private static bool IsElementLoaded(IWebElement element) {
            return element != null && element.IsPresent();
        }

    }

}
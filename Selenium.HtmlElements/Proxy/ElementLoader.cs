using System;
using System.Collections.Generic;

using HtmlElements.Locators;

using OpenQA.Selenium;

using HtmlElements.Extensions;

namespace HtmlElements.Proxy {

    internal class ElementLoader : CachedElementLoader<IWebElement> {

        public ElementLoader(IElementLocator locator) : base(locator.FindElement, IsElementLoaded) {
            IgnoredExceptionTypes = new List<Type> {typeof(StaleElementReferenceException)};
        }

        private static bool IsElementLoaded(IWebElement element) {
            return element != null && element.IsPresent();
        }

    }

}
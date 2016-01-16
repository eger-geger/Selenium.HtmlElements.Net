using System;
using System.Collections.Generic;
using HtmlElements.Extensions;
using HtmlElements.Locators;
using OpenQA.Selenium;

namespace HtmlElements.Proxy {

    internal class ElementLoader : CachedElementLoader<IWebElement> {

        public ElementLoader(IElementLocator locator, Boolean cache) : base(locator.FindElement, IsElementLoaded) {
            UseCash = cache;
            
            IgnoredExceptionTypes = new List<Type> {
                typeof(StaleElementReferenceException)
            };
        }

        private static bool IsElementLoaded(IWebElement element) {
            return element != null && element.IsPresent();
        }

    }

}
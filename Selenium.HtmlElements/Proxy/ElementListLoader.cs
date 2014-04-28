using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using HtmlElements.Locators;

using OpenQA.Selenium;

namespace HtmlElements.Proxy {

    internal class ElementListLoader : CachedElementLoader<ReadOnlyCollection<IWebElement>> {

        private readonly Func<ReadOnlyCollection<IWebElement>> _findElements;

        public ElementListLoader(IElementLocator locator, Boolean usecash = false) {
            if (locator == null) throw new ArgumentNullException("locator");

            _findElements = locator.FindElements;
            
            UseCash = usecash;

            IgnoredExceptionTypes = new List<Type> {
                typeof(StaleElementReferenceException)
            };
        }

        protected override bool IsLoaded(ReadOnlyCollection<IWebElement> list) {
            return list != null;
        }

        protected override ReadOnlyCollection<IWebElement> DoLoad() {
            return _findElements();
        }
    }

}
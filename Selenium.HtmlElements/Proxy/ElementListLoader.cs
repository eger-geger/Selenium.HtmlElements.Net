using System;
using System.Collections;
using System.Collections.Generic;

using HtmlElements.Locators;

using OpenQA.Selenium;

namespace HtmlElements.Proxy {

    internal class ElementListLoader : CachedElementLoader<IList> {

        private readonly Type _elementType;
        private readonly Func<IList<IWebElement>> _findElements;

        public ElementListLoader(Type type, IElementLocator locator) {
            if (type == null) throw new ArgumentNullException("type");
            if (locator == null) throw new ArgumentNullException("locator");

            _findElements = locator.FindElements;
            _elementType = type;

            IgnoredExceptionTypes = new List<Type>{typeof(StaleElementReferenceException)};
        }

        protected override bool IsLoaded(IList list) {
            return list != null && list.Count > 0;
        }

        protected override IList DoLoad() {
            var typedList = CreateTypedList(_elementType);

            foreach (var element in _findElements()) {
                typedList.Add(ElementFactory.Create(_elementType, new SelfLocator(element), true));
            }

            return typedList;
        }

        private static IList CreateTypedList(Type type) {
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(type)) as IList;
        }

    }

}
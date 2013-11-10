using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Locators {

    internal class ListItemLocator : IElementLocator {

        private readonly int _elementIndex;
        private readonly IElementLocator _wrappedLocator;

        public ListItemLocator(IElementLocator wrappedLocator, int elementIndex) {
            _wrappedLocator = wrappedLocator;
            _elementIndex = elementIndex;
        }

        public IWebElement FindElement() {
            return _wrappedLocator.FindElements()[_elementIndex];
        }

        public ReadOnlyCollection<IWebElement> FindElements() {
            return _wrappedLocator.FindElements();
        }

    }

}
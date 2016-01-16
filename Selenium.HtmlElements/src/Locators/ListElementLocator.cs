using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.Locators {

    internal class ListElementLocator : IElementLocator {

        private readonly Func<ReadOnlyCollection<IWebElement>> _findElements;

        private readonly Int32 _elementIndex;

        public ListElementLocator(Func<ReadOnlyCollection<IWebElement>> findElements, int elementIndex) {
            _findElements = findElements;
            _elementIndex = elementIndex;
        }

        public IWebElement FindElement() {
            try {
                return FindElements()[_elementIndex];
            } catch (ArgumentOutOfRangeException) {
                throw new NoSuchElementException(String.Format("element with index [{0}] not found", _elementIndex));
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements() {
            return _findElements();
        }

    }

}
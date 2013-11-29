using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Locators {

    internal class SelfLocator : IElementLocator {

        private readonly List<IWebElement> _elements;

        public SelfLocator(IWebElement element) : this(new List<IWebElement> {element}) {}

        public SelfLocator(List<IWebElement> elements) {
            _elements = elements;
        }

        public IWebElement FindElement() {
            return _elements[0];
        }

        public ReadOnlyCollection<IWebElement> FindElements() {
            return _elements.AsReadOnly();
        }

    }

}
using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Internal {

    internal class SelfLocator : IElementLocator {

        private readonly IWebElement _webElement;

        public SelfLocator(IWebElement webElement) {
            _webElement = webElement;
        }

        public IWebElement FindElement() {
            return _webElement;
        }

        public ReadOnlyCollection<IWebElement> FindElements() {
            return new List<IWebElement> {_webElement}.AsReadOnly();
        }

    }

}
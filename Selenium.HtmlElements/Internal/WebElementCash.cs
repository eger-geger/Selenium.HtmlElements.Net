using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

using Selenium.HtmlElements.Elements;

namespace Selenium.HtmlElements.Internal {

    internal class WebElementCash : LoadableComponent<WebElementCash>, IWrapsElement {

        private readonly IElementLocator _locator;

        public WebElementCash(IElementLocator locator) {
            _locator = locator;
        }

        public IWebElement WrappedElement { get; private set; }

        protected override void ExecuteLoad() {
            WrappedElement = _locator.FindElement();
        }

        protected override void HandleLoadError(WebDriverException ex) {
            if (!(ex is StaleElementReferenceException)) throw ex;

            Thread.Sleep(500);

            TryLoad();
        }

        protected override bool EvaluateLoadedStatus() {
            return WrappedElement != null && WrappedElement.IsPresent();
        }

        public override string ToString() {
            return string.Format("Cashed element found by: {0}", _locator);
        }

    }

}
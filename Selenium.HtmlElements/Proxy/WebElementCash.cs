using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

using Selenium.HtmlElements.Elements;
using Selenium.HtmlElements.Locators;

namespace Selenium.HtmlElements.Proxy {

    internal class WebElementCash : LoadableComponent<WebElementCash>, IWrapsElement, IWrapsDriver {

        private readonly IElementLocator _locator;

        public WebElementCash(IElementLocator locator) {
            _locator = locator;
        }

        public IWebDriver WrappedDriver {
            get {
                var iWrapsDriver = (WrappedElement as IWrapsDriver);

                if (iWrapsDriver == null) throw new WebDriverException("Cannot extract wrapped IWebDriver instance");

                return iWrapsDriver.WrappedDriver;
            }
        }

        public IWebElement WrappedElement { get; private set; }

        protected override void ExecuteLoad() {
            WrappedElement = _locator.FindElement();
            if (WrappedElement is IWrapsElement) {
                WrappedElement = (WrappedElement as IWrapsElement).WrappedElement;
            }
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
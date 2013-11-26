using System;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

using Selenium.HtmlElements.Elements;

namespace Selenium.HtmlElements.Internal {

    internal class ElementLoader : LoadableComponent<ElementLoader>, IWrapsElement, IWrapsDriver {

        private readonly IElementLocator _locator;

        private readonly bool _useCash;

        public ElementLoader(IElementLocator locator, bool useCash) {
            _locator = locator;
            _useCash = useCash;
        }

        public IWebDriver WrappedDriver {
            get { return UnwrapDriver(WrappedElement); }
        }

        public IWebElement WrappedElement { get; private set; }

        private IWebDriver UnwrapDriver(IWebElement webElement) {
            var driverWrapper = webElement as IWrapsDriver;

            if (driverWrapper != null) return driverWrapper.WrappedDriver;

            throw new ArgumentException("Not a driver wrapper", "webElement");
        }

        protected override void ExecuteLoad() {
            WrappedElement = UnwrapElement(_locator.FindElement());
        }

        private IWebElement UnwrapElement(IWebElement webElement) {
            var wrapper = webElement as IWrapsElement;

            if (wrapper == null) return webElement;

            return UnwrapElement(wrapper.WrappedElement);
        }

        public override ElementLoader Load() {
            if (!_useCash) WrappedElement = null;

            return base.Load();
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
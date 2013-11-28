using System;
using System.Collections.ObjectModel;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

using Selenium.HtmlElements.Elements;

namespace Selenium.HtmlElements.Internal {

    internal class ElementLoader : LoadableComponent<ElementLoader>, IWrapsElement, IWrapsDriver {

        public readonly IElementLocator Locator;
        public readonly bool UseCach;

        public ElementLoader(IElementLocator locator, bool useCach) {
            Locator = locator;
            UseCach = useCach;
        }

        public IWebDriver WrappedDriver {
            get { return UnwrapDriver(WrappedElement); }
        }

        public IWebElement WrappedElement {
            get { return WrappedElementList[0]; }
        }

        public ReadOnlyCollection<IWebElement> WrappedElementList { get; private set; } 

        private IWebDriver UnwrapDriver(IWebElement webElement) {
            var driverWrapper = UnwrapElement(webElement) as IWrapsDriver;

            if (driverWrapper != null) return driverWrapper.WrappedDriver;

            throw new ArgumentException("Not a driver wrapper", "webElement");
        }

        private IWebElement UnwrapElement(IWebElement webElement) {
            var wrapper = webElement as IWrapsElement;

            if (wrapper != null) return UnwrapElement(wrapper.WrappedElement);

            return webElement;
        }

        protected override void ExecuteLoad() {
            WrappedElementList = Locator.FindElements();
        }

        public override ElementLoader Load() {
            if (!UseCach) WrappedElementList = null;

            return base.Load();
        }

        protected override void HandleLoadError(WebDriverException ex) {
            if (!(ex is StaleElementReferenceException)) throw ex;

            Thread.Sleep(500);

            TryLoad();
        }

        protected override bool EvaluateLoadedStatus() {
            return WrappedElementList != null && WrappedElement.IsPresent();
        }

        public override string ToString() {
            return string.Format("Cashed element found by: {0}", Locator);
        }

    }

}
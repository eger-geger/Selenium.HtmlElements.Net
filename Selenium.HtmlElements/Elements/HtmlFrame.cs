using System;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    public class HtmlFrame : HtmlElement {

        public HtmlFrame(IWebElement wrapped) : base(wrapped) {}

        public string Src {
            get { return GetAttribute("src"); }
            set { this.SetAttribute("src", value); }
        }

        public void DoInFrame(Action action) {
            var webDriver = WrappedDriver;

            lock (webDriver) {
                webDriver.SwitchTo().Frame(this);
                action.Invoke();
                webDriver.SwitchTo().DefaultContent();
            }
        }

    }

}
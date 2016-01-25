using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlFrame : HtmlElement {

        public HtmlFrame(IWebElement wrapped) : base(wrapped) {}

        public string Src {
            get { return GetAttribute("src"); }
            set { this.SetAttribute("src", value); }
        }

        public void DoInFrame(Action action) {
            var webDriver = WrappedDriver;

            lock (webDriver) {
                webDriver.SwitchTo().Frame(WrappedElement);
                action.Invoke();
                webDriver.SwitchTo().DefaultContent();
            }
        }

    }

}
using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    internal class HtmlLabel : HtmlElement {

        public HtmlLabel(IWebElement wrapped) : base(wrapped) {}

        public string For {
            get { return GetAttribute("for"); }
            set { this.SetAttribute("for", value); }
        }

    }

}
using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    public class HtmlInput : HtmlControl {

        public HtmlInput(IWebElement wrapped) : base(wrapped) {}

        public string Type {
            get { return GetAttribute("type"); }
            set { this.SetAttribute("type", value); }
        }

        public string InputSize {
            get { return GetAttribute("size"); }
            set { this.SetAttribute("size", value); }
        }

    }

}
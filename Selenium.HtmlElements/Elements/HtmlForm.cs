using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    //TODO: Add specific properties and methods
    public class HtmlForm : HtmlElement {

        public HtmlForm(IWebElement wrapped) : base(wrapped) {}

        public string Action {
            get { return GetAttribute("action"); }
            set { this.SetAttribute("action", value); }
        }

        public string Method {
            get { return GetAttribute("method"); }
            set { this.SetAttribute("method", value); }
        }

    }

}
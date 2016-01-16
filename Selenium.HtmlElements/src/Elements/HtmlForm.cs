using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

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

        public string Enctype {
            get { return GetAttribute("enctype"); }
            set { this.SetAttribute("enctype", value); }
        }

        public string Target {
            get { return GetAttribute("target"); }
            set { this.SetAttribute("target", value); }
        }

    }

}
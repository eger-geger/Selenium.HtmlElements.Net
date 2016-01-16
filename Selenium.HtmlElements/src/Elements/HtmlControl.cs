using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public abstract class HtmlControl : HtmlElement {

        protected HtmlControl(IWebElement wrapped) : base(wrapped) {}

        public string Value {
            get { return GetAttribute("value"); }
            set { this.SetAttribute("value", value); }
        }

        public bool Disabled {
            get { return this.HasAttribute("disabled"); }
            set {
                if (value) this.SetAttribute("disabled", "disabled");
                else this.RemoveAttribute("disabled");
            }
        }

    }

}
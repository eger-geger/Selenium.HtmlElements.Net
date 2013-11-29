using OpenQA.Selenium;

using HtmlElements.Extensions;

namespace HtmlElements.Elements {

    public class HtmlLabel : HtmlElement {

        public HtmlLabel(IWebElement wrapped) : base(wrapped) {}

        public string For {
            get { return GetAttribute("for"); }
            set { this.SetAttribute("for", value); }
        }

    }

}
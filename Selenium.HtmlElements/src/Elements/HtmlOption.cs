using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlOption : HtmlControl {

        public HtmlOption(IWebElement webElement) : base(webElement) {}

        public string Label {
            get { return GetAttribute("label"); }
            set { this.SetAttribute("label", value); }
        }

        public void Select() {
            this.Do(Click).Until(self => Selected);
        }

    }

}
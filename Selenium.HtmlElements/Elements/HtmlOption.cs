using OpenQA.Selenium;

using HtmlElements.Actions;
using HtmlElements.Extensions;

namespace HtmlElements.Elements {

    public class HtmlOption : HtmlControl {

        public HtmlOption(IWebElement wrapped) : base(wrapped) {}

        public string Label {
            get { return GetAttribute("label"); }
            set { this.SetAttribute("label", value); }
        }

        public void Select() {
            this.Do(Click).Until(self => Selected);
        }

    }

}
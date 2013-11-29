using OpenQA.Selenium;

using Selenium.HtmlElements.Actions;
using Selenium.HtmlElements.Extensions;

namespace Selenium.HtmlElements.Elements {

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
using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    internal class HtmlOption : HtmlControl {

        public HtmlOption(IWebElement wrapped) : base(wrapped) {}

        public string Label {
            get { return GetAttribute("label"); }
            set { this.SetAttribute("label", value); }
        }

        public new bool Selected {
            get { return base.Selected; }
            set { this.Do(self => self.Click()).Until(self => (self as IWebElement).Selected == value); }
        }

    }

}
using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    public class HtmlCheckBox : HtmlInput {

        private const string AttrChecked = "checked";

        public HtmlCheckBox(IWebElement wrapped) : base(wrapped) {}

        public bool Checked {
            get { return this.HasAttribute(AttrChecked); }
            set { this.Do(self => self.Click()).Until(self => (self as IWebElement).Selected == value); }
        }

    }

}
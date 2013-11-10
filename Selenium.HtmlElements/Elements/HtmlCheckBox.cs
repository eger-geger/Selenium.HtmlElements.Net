using System;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    public class HtmlCheckBox : HtmlInput {

        private const string AttrChecked = "checked";

        public HtmlCheckBox(IWebElement wrapped) : base(wrapped) {}

        public bool Checked {
            get { return this.HasAttribute(AttrChecked); }
            set { this.WaitForState(IsChecked(value)); }
        }

        private static Predicate<HtmlCheckBox> IsChecked(bool state) {
            return self => {
                if (self.Checked != state) self.Click();
                return self.Checked == state;
            };
        }

    }

}
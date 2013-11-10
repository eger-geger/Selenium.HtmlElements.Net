using System;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    public class HtmlSelectOption : HtmlControl {

        public HtmlSelectOption(IWebElement wrapped) : base(wrapped) {}

        public new bool Selected {
            get { return base.Selected; }
            set { this.WaitForState(IsSelected(value)); }
        }

        private static Predicate<HtmlSelectOption> IsSelected(bool state) {
            return self => {
                if (self.Selected != state) self.Click();
                return self.Selected == state;
            };
        }

    }

}
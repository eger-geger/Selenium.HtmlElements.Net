using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    public class HtmlSelectOption : HtmlControl {

        public HtmlSelectOption(IWebElement wrapped) : base(wrapped) {}

        public new bool Selected {
            get { return base.Selected; }
            set { this.Do(e => e.Click()).Until(e => (e as IWebElement).Selected == value); }
        }

    }

}
using HtmlElements.Extensions;

using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlCheckBox : HtmlInput {

        public HtmlCheckBox(IWebElement wrapped) : base(wrapped) {}

        public bool Checked {
            get { return Selected; }
            set { this.Do(Click).Until(self => Selected == value); }
        }

    }

}
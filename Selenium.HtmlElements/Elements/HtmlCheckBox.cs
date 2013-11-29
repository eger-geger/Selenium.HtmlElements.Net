using OpenQA.Selenium;

using Selenium.HtmlElements.Actions;
using Selenium.HtmlElements.Extensions;

namespace Selenium.HtmlElements.Elements {

    public class HtmlCheckBox : HtmlInput {

        private const string AttrChecked = "checked";

        public HtmlCheckBox(IWebElement wrapped) : base(wrapped) {}

        public bool Checked {
            get { return this.HasAttribute(AttrChecked); }
            set { this.Do(Click).Until(self => Checked == value); }
        }

    }

}
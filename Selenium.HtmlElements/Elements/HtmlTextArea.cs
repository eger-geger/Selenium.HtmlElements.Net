using OpenQA.Selenium;

using Selenium.HtmlElements.Extensions;

namespace Selenium.HtmlElements.Elements {

    public class HtmlTextArea : HtmlControl, ITextControl {

        public HtmlTextArea(IWebElement wrapped) : base(wrapped) {}

        public string Cols {
            get { return GetAttribute("cols"); }
            set { this.SetAttribute("cols", value); }
        }

        public string Rows {
            get { return GetAttribute("rows"); }
            set { this.SetAttribute("rows", value); }
        }

    }

}
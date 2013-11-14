using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    public class HtmlTextArea : HtmlControl {

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
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlTextArea : HtmlControl, ITextControl {

        public HtmlTextArea(IWebElement webDriverOrWrapper) : base(webDriverOrWrapper) {}

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
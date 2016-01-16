using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlInput : HtmlControl, ITextControl {

        public HtmlInput(IWebElement wrapped) : base(wrapped) {}

        public string Type {
            get { return GetAttribute("type"); }
            set { this.SetAttribute("type", value); }
        }

        public string InputSize {
            get { return GetAttribute("size"); }
            set { this.SetAttribute("size", value); }
        }

        public string MaxLength {
            get { return GetAttribute("maxlength"); }
            set { this.SetAttribute("maxlength", value); }
        }

        public string Src {
            get { return GetAttribute("src"); }
            set { this.SetAttribute("src", value); }
        }

    }

}
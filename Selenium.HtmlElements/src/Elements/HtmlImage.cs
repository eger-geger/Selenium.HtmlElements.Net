using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlImage : HtmlElement {

        public HtmlImage(IWebElement wrapped) : base(wrapped) {}

        public string Alt {
            get { return GetAttribute("alt"); }
            set { this.SetAttribute("alt", value); }
        }

        public string Src {
            get { return GetAttribute("src"); }
            set { this.SetAttribute("src", value); }
        }

        public string Height {
            get { return GetAttribute("height"); }
            set { this.SetAttribute("height", value); }
        }

        public string Width {
            get { return GetAttribute("width"); }
            set { this.SetAttribute("width", value); }
        }

    }

}
using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    /// <summary>
    ///     Models image DOM element and provides most commonly used image attributes.
    /// </summary>
    public class HtmlImage : HtmlElement {

        ///<summary>
        ///     Initializes new instance of HTML element by calling base class constructor
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        public HtmlImage(IWebElement webElement) : base(webElement) {}

        /// <summary>
        ///     Gets or sets 'alt' attribute of the underlying image element or null if it does not exist
        /// </summary>
        public string Alt {
            get { return GetAttribute("alt"); }
            set { this.SetAttribute("alt", value); }
        }

        /// <summary>
        ///     Gets or sets 'src' attribute of the underlying image element or null if it does not exist
        /// </summary>
        public string Src {
            get { return GetAttribute("src"); }
            set { this.SetAttribute("src", value); }
        }

        /// <summary>
        ///     Gets or sets 'height' attribute of the underlying image element or null if it does not exist
        /// </summary>
        public string Height {
            get { return GetAttribute("height"); }
            set { this.SetAttribute("height", value); }
        }

        /// <summary>
        ///     Gets or sets 'width' attribute of the underlying image element or null if it does not exist
        /// </summary>
        public string Width {
            get { return GetAttribute("width"); }
            set { this.SetAttribute("width", value); }
        }

    }

}
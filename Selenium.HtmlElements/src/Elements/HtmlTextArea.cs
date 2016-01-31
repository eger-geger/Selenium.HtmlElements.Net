using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    /// <summary>
    ///     Models text area control and exposes it's attributes
    /// </summary>
    public class HtmlTextArea : HtmlControl {

        /// <summary>
        ///     Initializes new instance of HTML element by calling base class constructor
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown when <paramref name="webElement" /> does not wrap WebDriver
        /// </exception>
        public HtmlTextArea(IWebElement webElement) : base(webElement) {}

        /// <summary>
        ///     Gets or sets 'cols' attribute of the underlying text area or null if it does not exist
        /// </summary>
        public string Cols {
            get { return GetAttribute("cols"); }
            set { this.SetAttribute("cols", value); }
        }

        /// <summary>
        ///     Gets or sets 'rows' attribute of the underlying text area or null if it does not exist
        /// </summary>
        public string Rows {
            get { return GetAttribute("rows"); }
            set { this.SetAttribute("rows", value); }
        }

    }

}
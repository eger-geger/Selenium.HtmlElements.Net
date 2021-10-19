using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///     Models HTML input element of any type and exposes it's attributes as properties
    /// </summary>
    public class HtmlInput : HtmlControl
    {
        ///<summary>
        ///     Initializes new instance of HTML element by calling base class constructor
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        public HtmlInput(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        ///     Gets or sets 'size' attribute of the underlying input element or null if it does not exist
        /// </summary>
        public string InputSize
        {
            get => GetDomAttribute("size");
            set => this.SetAttribute("size", value);
        }

        /// <summary>
        ///     Gets or sets 'maxlength' attribute of the underlying input element or null if it does not exist
        /// </summary>
        public string MaxLength
        {
            get => GetDomAttribute("maxlength");
            set => this.SetAttribute("maxlength", value);
        }

        /// <summary>
        ///     Gets or sets 'src' attribute of the underlying input element or null if it does not exist
        /// </summary>
        public string Src
        {
            get => GetDomAttribute("src");
            set => this.SetAttribute("src", value);
        }

        /// <summary>
        ///     Gets or sets 'type' attribute of the underlying input element or null if it does not exist
        /// </summary>
        public string Type
        {
            get => GetDomAttribute("type");
            set => this.SetAttribute("type", value);
        }
    }
}
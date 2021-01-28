using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///      Models iframe DOM element and provides and exposes it's attributes as properties.
    /// </summary>
    public class HtmlFrame : HtmlElement
    {
        ///<summary>
        ///     Initializes new instance of HTML element by calling base class constructor
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        public HtmlFrame(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        ///     Gets or sets 'src' attribute of the underlying iframe or null if it does not exist
        /// </summary>
        public string SourceURL
        {
            get { return GetAttribute("src"); }
            set { this.SetAttribute("src", value); }
        }
    }
}
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///     Models HTML select, input or text area elements
    /// </summary>
    public abstract class HtmlControl : HtmlElement
    {
        /// <summary>
        ///     Initializes new instance of HTML element by calling base class constructor
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        protected HtmlControl(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        ///     Disabled/enabled control state
        /// </summary>
        public bool Disabled
        {
            get { return this.HasAttribute("disabled"); }
            set
            {
                if (value)
                {
                    this.SetAttribute("disabled", "disabled");
                }
                else
                {
                    this.RemoveAttribute("disabled");
                }
            }
        }

        /// <summary>
        ///     Value assigned to control
        /// </summary>
        public string Value
        {
            get { return GetAttribute("value"); }
            set { this.SetAttribute("value", value); }
        }
    }
}
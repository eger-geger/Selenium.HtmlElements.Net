using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///     Models label DOM element and exposes it's specific attributes as properties
    /// </summary>
    public class HtmlLabel : HtmlElement
    {
        ///<summary>
        ///     Initializes new instance of HTML element by calling base class constructor
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="webElement"/> does not wrap WebDriver
        /// </exception>
        public HtmlLabel(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        ///     Gets or sets 'for' attribute of the underlying label or null if it does not exist
        /// </summary>
        public string For
        {
            get => GetDomAttribute("for");
            set => this.SetAttribute("for", value);
        }
    }
}
using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///     Models form DOM element
    /// </summary>
    public class HtmlForm : HtmlElement
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
        public HtmlForm(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        ///     Gets or sets 'actions' attribute of the underlying form or null if it does not exist
        /// </summary>
        public string Action
        {
            get => GetAttribute("action");
            set => this.SetAttribute("action", value);
        }

        /// <summary>
        ///     Gets or sets 'enctype' attribute of the underlying form or null if it does not exist
        /// </summary>
        public string Enctype
        {
            get => GetAttribute("enctype");
            set => this.SetAttribute("enctype", value);
        }

        /// <summary>
        ///     Gets or sets 'method' attribute of the underlying form or null if it does not exist
        /// </summary>
        public string Method
        {
            get => GetAttribute("method");
            set => this.SetAttribute("method", value);
        }

        /// <summary>
        ///     Gets or sets 'target' attribute of the underlying form or null if it does not exist
        /// </summary>
        public string Target
        {
            get => GetAttribute("target");
            set => this.SetAttribute("target", value);
        }
    }
}
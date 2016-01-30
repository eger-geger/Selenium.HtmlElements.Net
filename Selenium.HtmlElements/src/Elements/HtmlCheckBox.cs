using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///     Models HTML input with checkbox or radio button type
    /// </summary>
    public class HtmlCheckBox : HtmlInput
    {
        ///<summary>
        ///     Initializes new instance of HTML element by calling base class constructors
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="webElement"/> does not wrap WebDriver
        /// </exception>
        public HtmlCheckBox(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        ///     Specifies whether checkbox or radio button is selected and allows to change it
        /// </summary>
        public bool Checked
        {
            get { return Selected; }
            set { this.Do(Click).Until(self => Selected == value); }
        }
    }
}
using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///      Models iframe DOM element and provides convenient method for performing actions in it.
    /// </summary>
    public class HtmlFrame : HtmlElement
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

        /// <summary>
        ///     Executes action switching the WebDriver context to current frame before execution and switching it back to default WebDriver context after.
        /// </summary>
        /// <param name="action">Action to execute</param>
        public void ExecuteInFrame(Action action)
        {
            var webDriver = WrappedDriver;

            webDriver.SwitchTo().Frame(WrappedElement);

            try
            {
                action.Invoke();
            }
            finally
            {
                webDriver.SwitchTo().DefaultContent();    
            }
        }

        /// <summary>
        ///     Executes action switching the WebDriver context to current frame before execution and switching it back to default WebDriver context after.
        /// </summary>
        /// <typeparam name="TResult">Action return type</typeparam>
        /// <param name="action">Action to execute</param>
        /// <returns>Value returned by action</returns>
        public TResult ExecuteInFrame<TResult>(Func<TResult> action)
        {
            TResult returnValue = default(TResult);

            ExecuteInFrame(() => returnValue = action());

            return returnValue;
        }
    }
}
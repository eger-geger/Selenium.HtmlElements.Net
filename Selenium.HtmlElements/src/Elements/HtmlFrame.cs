using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///      Represents iframe and can be used to describe it's content.
    /// </summary>
    public class HtmlFrame : HtmlElement
    {
        public HtmlFrame(IWebElement wrapped) : base(wrapped)
        {
        }

        /// <summary>
        ///     Specifies the address of the embedded document
        /// </summary>
        public string SourceURL
        {
            get { return GetAttribute("src"); }
            set { this.SetAttribute("src", value); }
        }

        /// <summary>
        ///     Executes action switching the WebDriver context to current frame before action execution 
        ///     and switching it back to default WebDriver context after execution
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
        ///     Executes action switching the WebDriver context to current frame before action execution 
        ///     and switching it back to default WebDriver context after execution
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
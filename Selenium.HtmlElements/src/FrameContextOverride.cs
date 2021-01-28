using System;
using HtmlElements.Elements;
using HtmlElements.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements
{
    /// <summary>
    ///     Allows temporary switching web driver context to frame and switch it back to default context once get disposed.
    /// </summary>
    public class FrameContextOverride : IDisposable, IWrapsDriver
    {
        private readonly IWebDriver _wrappedDriver;

        /// <summary>
        ///     Switch WebDriver context to frame wrapped by given element
        /// </summary>
        /// <param name="frameElement">Frame which will became active</param>
        public FrameContextOverride(HtmlFrame frameElement) : this(frameElement.WrappedDriver, frameElement.ToRawWebElement())
        {
        }

        /// <summary>
        ///     Switch WebDriver context to frame wrapped by given element
        /// </summary>
        /// <param name="webDriver">Target WebDriver</param>
        /// <param name="frameElement">Frame which will became active</param>
        public FrameContextOverride(IWebDriver webDriver, IWebElement frameElement)
        {
            if (frameElement == null)
            {
                throw new ArgumentNullException(nameof(frameElement));
            }

            _wrappedDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
                
            webDriver.SwitchTo().Frame(frameElement);
        }

        /// <summary>
        ///     Driver instance which context get switched
        /// </summary>
        public IWebDriver WrappedDriver
        {
            get { return _wrappedDriver; }
        }

        /// <summary>
        ///     Switch wrapped driver context to default content
        /// </summary>
        public void Dispose()
        {
            _wrappedDriver.SwitchTo().DefaultContent();
        }
    }
}
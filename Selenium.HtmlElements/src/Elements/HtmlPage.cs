using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace HtmlElements.Elements {

    /// <summary>
    ///     Models a wed page loaded in browser and exposes some useful methods and properties
    /// </summary>
    public class HtmlPage : WebDriverWrapper
    {
        /// <summary>
        ///     Initializes new instance of a page by calling base class constructor
        /// </summary>
        /// <param name="webDriverOrWrapper">
        ///     <see cref="IWebDriver"/>, <see cref="IWebElement"/> or anything else 
        ///     which is wrapping WebDriver instance and can be used for locating elements
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="webDriverOrWrapper"/> is not WebDriver and it does not wrap WebDriver
        /// </exception>
        public HtmlPage(ISearchContext webDriverOrWrapper) : base(webDriverOrWrapper) {
        }

        /// <summary>
        ///     Loading status of the current document loaded or being loading in browser
        /// </summary>
        public DocumentReadyState ReadyState {
            get { 
                var readyState = (String) ExecuteScript("return document.readyState;");

                switch (readyState) {
                    case "uninitialized": 
                        return DocumentReadyState.Uninitialized;
                    case "loading": 
                        return DocumentReadyState.Loading;
                    case "interactive": 
                        return DocumentReadyState.Interactive;
                    case "loaded":
                        return DocumentReadyState.Loaded;
                    case "complete": 
                        return DocumentReadyState.Complete;
                }

                throw new InvalidOperationException(
                    String.Format("Unexpected document state: {0}", readyState)
                );
            }
        }

        /// <summary>
        ///     HTML element which represents body tag of the current page
        /// </summary>
        [FindsBy(How = How.TagName, Using = "body"), CacheLookup]
        public HtmlElement Body { get; private set; }

        /// <summary>
        ///     Title of the current page
        /// </summary>
        public string Title {
            get { return WrappedDriver.Title; }
        }

        /// <summary>
        ///     Source code of the current page
        /// </summary>
        /// <remarks>
        ///     If the page has been modified after loading (for example, by JavaScript) there is no guarantee that the returned text is that of the modified page.  
        ///     Please consult the documentation of the particular driver being used to determine whether the returned text reflects the current state of the page or the text last sent by the web server. 
        ///     The page source returned is a representation of the underlying DOM: do not expect it to be formatted or escaped in the same way as the response sent from the web server.
        /// </remarks>
        public string Source {
            get { return WrappedDriver.PageSource; }
        }

        /// <summary>
        ///     URL of the current page
        /// </summary>
        public string CurrentUrl {
            get { return WrappedDriver.Url; }
        }

        /// <summary>
        ///     Reload the current page
        /// </summary>
        public void Refresh() {
            WrappedDriver.Navigate().Refresh();
        }
    }
}
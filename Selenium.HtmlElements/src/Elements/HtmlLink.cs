using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    /// <summary>
    ///     Models HTML link element and exposes it's attributes as properties
    /// </summary>
    public class HtmlLink : HtmlElement
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
        public HtmlLink(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        ///     Absolute URL which link is pointing to
        /// </summary>
        public string Url {
            get {
                var windowLocation = new Uri(WrappedDriver.Url);

                var linkHref = Href;

                return linkHref.Contains(Uri.UriSchemeHttp) || linkHref.Contains(Uri.UriSchemeHttps)
                    ? linkHref : String.Format("{0}://{1}{2}", windowLocation.Scheme, windowLocation.Host, linkHref);
            }
        }

        /// <summary>
        ///     Gets or sets 'href' attribute of the underlying link element or null if it does not exist
        /// </summary>
        public string Href {
            get { return GetAttribute("href"); }
            set { this.SetAttribute("href", value); }
        }

        /// <summary>
        ///     Gets or sets 'target' attribute of the underlying link element or null if it does not exist
        /// </summary>
        public string Target {
            get { return GetAttribute("target"); }
            set { this.SetAttribute("target", value); }
        }
    }

}
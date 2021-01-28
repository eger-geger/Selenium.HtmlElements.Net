using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
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
        public HtmlLink(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        ///     Gets or sets 'href' attribute of the underlying link element or null if it does not exist
        /// </summary>
        public string Href
        {
            get => GetAttribute("href");
            set => this.SetAttribute("href", value);
        }

        /// <summary>
        ///     Gets or sets 'target' attribute of the underlying link element or null if it does not exist
        /// </summary>
        public string Target
        {
            get => GetAttribute("target");
            set => this.SetAttribute("target", value);
        }

        /// <summary>
        ///     Absolute URL which link is pointing to
        /// </summary>
        public string Url
        {
            get
            {
                var windowLocation = new Uri(WrappedDriver.Url);

                var linkHref = Href;

                return linkHref.Contains(Uri.UriSchemeHttp) || linkHref.Contains(Uri.UriSchemeHttps)
                    ? linkHref
                    : string.Format("{0}://{1}{2}", windowLocation.Scheme, windowLocation.Host, linkHref);
            }
        }
    }
}
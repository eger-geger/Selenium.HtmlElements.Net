using System;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlLink : HtmlElement {

        public const string TargetBlank = "_blank";
        public const string TargetParent = "_parent";
        public const string TargetSelf = "_self";
        public const string TargetTop = "_top";

        public HtmlLink(IWebElement wrapped) : base(wrapped) {}

        public string Url {
            get {
                var windowLocation = new Uri(WrappedDriver.Url);

                var linkHref = Href;

                return linkHref.Contains(Uri.UriSchemeHttp) || linkHref.Contains(Uri.UriSchemeHttps)
                    ? linkHref : String.Format("{0}://{1}{2}", windowLocation.Scheme, windowLocation.Host, linkHref);
            }
        }

        public string Href {
            get { return GetAttribute("href"); }
            set { this.SetAttribute("href", value); }
        }

        public string Target {
            get { return GetAttribute("target"); }
            set { this.SetAttribute("target", value); }
        }

        public TReturn Open<TReturn>() where TReturn : class {
            var wd = WrappedDriver;

            Click();

            return ElementActivator.Activate<TReturn>(wd);
        }

    }

}
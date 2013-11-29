using System;
using System.ComponentModel;
using System.Xml.Linq;

using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlPage : SearchContextWrapper {

        public enum DocumentReadyState {

            Uninitialized,
            Loading,
            Interactive,
            Complete

        }

        public HtmlPage(ISearchContext wrapped) : base(wrapped) {
            ElementActivator.Activate(this, wrapped);
        }

        public DocumentReadyState ReadyState {
            get { 
                var readyState = (string) ExecuteScript("return document.readyState;");

                switch (readyState) {
                    case "uninitialized": return DocumentReadyState.Uninitialized;
                    case "loading": return DocumentReadyState.Loading;
                    case "interactive": return DocumentReadyState.Interactive;
                    case "complete": return DocumentReadyState.Complete;
                }

                throw new InvalidOperationException("unexpected document state");
            }
        }

        public string Title {
            get { return WrappedDriver.Title; }
        }

        public string Source {
            get { return WrappedDriver.PageSource; }
        }

        public XDocument Document {
            get { return XDocument.Parse(Source); }
        }

        public string CurrentUrl {
            get { return WrappedDriver.Url; }
        }

        public void Refresh() {
            WrappedDriver.Navigate().Refresh();
        }

        public void DeleteAllCoockies() {
            WrappedDriver.Manage().Cookies.DeleteAllCookies();
        }
    }

}
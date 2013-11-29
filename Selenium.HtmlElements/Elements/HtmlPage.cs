using System.Xml.Linq;

using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlPage : SearchContextWrapper {

        public HtmlPage(ISearchContext wrapped) : base(wrapped) {
            ElementActivator.Activate(this, wrapped);
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
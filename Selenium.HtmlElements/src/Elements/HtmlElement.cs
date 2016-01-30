using System;
using System.Drawing;
using HtmlElements.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Elements {

    public class HtmlElement : WebDriverWrapper, IHtmlElement {

        private readonly IWebElement _wrappedElement;

        public HtmlElement(IWebElement webDriverOrWrapper) : base(webDriverOrWrapper) {
            _wrappedElement = (webDriverOrWrapper is HtmlElement) ? (webDriverOrWrapper as HtmlElement)._wrappedElement : webDriverOrWrapper;
        }

        public IWebElement ParentNode {
            get { return this.GetProperty("parentNode") as IWebElement; }
        }

        public IWebElement PreviousSibling {
            get { return this.GetProperty("previousSibling") as IWebElement; }
        }

        public IWebElement FirstChild {
            get { return this.GetProperty("firstChild") as IWebElement; }
        }

        public IWebElement LastChild {
            get { return this.GetProperty("lastChild") as IWebElement; }
        }

        public String InnerHtml {
            get { return this.GetProperty("innerHTML") as String; }
            set { this.SetPropery("innerHTML", value); }
        }

        public String TextContent {
            get { return this.GetProperty("textContent") as String; }
            set { this.SetPropery("textContent", value); }
        }

        public string Name {
            get { return GetAttribute("name"); }
            set { this.SetAttribute("name", value); }
        }

        public string Id {
            get { return GetAttribute("id"); }
            set { this.SetAttribute("id", value); }
        }

        public string Class {
            get { return GetAttribute("class"); }
            set { this.SetAttribute("class", value); }
        }

        public string Style {
            get { return GetAttribute("style"); }
            set { this.SetAttribute("style", value); }
        }

        public string Title {
            get { return GetAttribute("title"); }
            set { this.SetAttribute("title", value); }
        }

        public void Clear() {
            _wrappedElement.Clear();
        }

        public void SendKeys(string text) {
            _wrappedElement.SendKeys(text);
        }

        public void Submit() {
            _wrappedElement.Submit();
        }

        public void Click() {
            _wrappedElement.Click();
        }

        public string GetAttribute(string attributeName) {
            return _wrappedElement.GetAttribute(attributeName);
        }

        public string GetCssValue(string propertyName) {
            return _wrappedElement.GetCssValue(propertyName);
        }

        public IWebElement WrappedElement {
            get {
                return _wrappedElement is IWrapsElement
                    ? (_wrappedElement as IWrapsElement).WrappedElement
                    : _wrappedElement;
            }
        }

        public string TagName {
            get { return _wrappedElement.TagName; }
        }

        public string Text {
            get { return _wrappedElement.Text; }
        }

        public bool Enabled {
            get { return _wrappedElement.Enabled; }
        }

        public bool Selected {
            get { return _wrappedElement.Selected; }
        }

        public Point Location {
            get { return _wrappedElement.Location; }
        }

        public Size Size {
            get { return _wrappedElement.Size; }
        }

        public bool Displayed {
            get { return _wrappedElement.IsPresent() && _wrappedElement.Displayed; }
        }

    }
}
using System;
using System.Drawing;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Selenium.HtmlElements.Elements {

    public class HtmlElement : SearchContextWrapper, IHtmlElement {

        private readonly IWebElement _wrappedElement;

        public HtmlElement(IWebElement wrapped) : base(wrapped) {
            _wrappedElement = (wrapped is HtmlElement) ? (wrapped as HtmlElement)._wrappedElement : wrapped;
        }

        public IWebElement ParentNode {
            get { return this.GetDomElementProperty("parentNode") as IWebElement; }
        }

        public IWebElement PreviousSibling {
            get { return this.GetDomElementProperty("previousSibling") as IWebElement; }
        }

        public IWebElement FirstChild {
            get { return this.GetDomElementProperty("firstChild") as IWebElement; }
        }

        public IWebElement LastChild {
            get { return this.GetDomElementProperty("lastChild") as IWebElement; }
        }

        public String InnerHtml {
            get { return this.GetDomElementProperty("innerHTML") as String; }
            set { this.SetDomElementPropery("innerHTML", value); }
        }

        public String TextContent {
            get { return this.GetDomElementProperty("textContent") as String; }
            set { this.SetDomElementPropery("textContent", value); }
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
                return (_wrappedElement is IWrapsElement)
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

        /// <summary>
        ///     Allows to refer to element as <code>{self}</code> inside script
        /// </summary>
        public object ExecuteScriptOnSelf(string script, params object[] args) {
            var newArgs = new object[args.Length + 1];
            newArgs[args.Length] = WrappedElement;

            args.CopyTo(newArgs, 0);

            return ExecuteScript(script.Replace("{self}", string.Format("arguments[{0}]", args.Length)), newArgs);
        }

    }

}
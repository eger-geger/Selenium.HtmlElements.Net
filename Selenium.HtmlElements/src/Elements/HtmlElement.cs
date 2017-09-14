using System;
using System.Drawing;
using HtmlElements.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Elements
{

    /// <summary>
    ///     Models HTML DOM element providing access to common attributes and properties
    /// </summary>
    public class HtmlElement : WebDriverWrapper, IHtmlElement
    {

        private readonly IWebElement _wrappedElement;

        ///<summary>
        ///     Initializes new instance of HTML element by calling base class constructor
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="webElement"/> does not wrap WebDriver
        /// </exception>
        public HtmlElement(IWebElement webElement) : base(webElement)
        {
            _wrappedElement = webElement;
        }

        /// <summary>
        ///     A WebElement, representing the parent node of current element, or null if it has no parent
        /// </summary>
        public IWebElement ParentNode
        {
            get { return this.GetProperty<IWebElement>("parentNode"); }
        }

        /// <summary>
        ///     A previous node of current web element, in the same tree level
        /// </summary>
        public IWebElement PreviousSibling
        {
            get { return this.GetProperty<IWebElement>("previousSibling"); }
        }

        /// <summary>
        ///     The next node of current web element, in the same tree level
        /// </summary>
        public IWebElement NextSibling
        {
            get { return this.GetProperty<IWebElement>("nextSibling"); }
        }

        /// <summary>
        ///     First child node of the web element, as a web element
        /// </summary>
        public IWebElement FirstChild
        {
            get { return this.GetProperty<IWebElement>("firstChild"); }
        }

        /// <summary>
        ///     Last child node of the current element, as a web element
        /// </summary>
        public IWebElement LastChild
        {
            get { return this.GetProperty<IWebElement>("lastChild"); }
        }

        /// <summary>
        ///     HTML content of an element
        /// </summary>
        public String InnerHtml
        {
            get { return this.GetProperty<string>("innerHTML"); }
            set { this.SetPropery("innerHTML", value); }
        }

        /// <summary>
        ///     Returns or sets the text from the element.
        ///     On returning text, this property returns the value of all text nodes within the element node.
        ///     On setting text, this property removes all child nodes and replaces them with a single text node.
        /// </summary>
        public String TextContent
        {
            get { return this.GetProperty("textContent") as String; }
            set { this.SetPropery("textContent", value); }
        }

        /// <summary>
        ///     Gets or sets 'name' attribute of the underlying DOM element or null if it does not exist
        /// </summary>
        public string Name
        {
            get { return GetAttribute("name"); }
            set { this.SetAttribute("name", value); }
        }

        /// <summary>
        ///     Gets or sets 'id' attribute of the underlying DOM element or null if it does not exist
        /// </summary>
        public string Id
        {
            get { return GetAttribute("id"); }
            set { this.SetAttribute("id", value); }
        }

        /// <summary>
        ///    Gets or sets 'class' attribute of the underlying DOM element or null if it does not exist
        /// </summary>
        public string Class
        {
            get { return GetAttribute("class"); }
            set { this.SetAttribute("class", value); }
        }

        /// <summary>
        ///     Gets or sets 'style' attribute of the underlying DOM element or null if it does not exist
        /// </summary>
        public string Style
        {
            get { return GetAttribute("style"); }
            set { this.SetAttribute("style", value); }
        }

        /// <summary>
        ///     Gets or sets 'title' attribute of the underlying DOM element or null if it does not exist
        /// </summary>
        public string Title
        {
            get { return GetAttribute("title"); }
            set { this.SetAttribute("title", value); }
        }

        /// <summary>
        ///     Clears the content of this element.
        /// </summary>
        /// <remarks>
        ///     If this element is a text entry element, the method will clear the value. 
        ///     It has no effect on other elements. Text entry elements are defined as elements with INPUT or TEXTAREA tags.
        /// </remarks>
        public void Clear()
        {
            _wrappedElement.Clear();
        }

        /// <summary>
        ///     Simulates typing text into the element.
        /// </summary>
        /// <param name="text">The text to type into the element.</param>
        /// <remarks>
        ///     The text to be typed may include special characters like arrow keys, backspaces, function keys, and so on. 
        ///     Valid special keys are defined in <see cref="Keys"/>.
        /// </remarks>
        public void SendKeys(string text)
        {
            _wrappedElement.SendKeys(text);
        }

        /// <summary>
        ///     Submits this element to the web server.
        /// </summary>
        /// <remarks>
        ///     If this current element is a form, or an element within a form, then this will be submitted to the web server. 
        ///     If this causes the current page to change, then this method will block until the new page is loaded.
        /// </remarks>
        public void Submit()
        {
            _wrappedElement.Submit();
        }

        /// <summary>
        ///     Clicks this element.
        /// </summary>
        /// <remarks>
        ///     Click this element. If the click causes a new page to load, the method will attempt to block until the page has loaded. 
        ///     After calling the method, you should discard all references to this element unless you know that the element and the page will still be present.  
        ///     Otherwise, any further operations performed on this element will have an undefined behavior.
        ///     If this element is not clickable, then this operation is ignored. 
        ///     This allows you to simulate a users to accidentally missing the target when clicking.
        /// </remarks>
        public void Click()
        {
            _wrappedElement.Click();
        }

        /// <summary>
        ///     Gets the value of the specified attribute for this element.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The attribute's current value. Returns a null if the value is not set.</returns>
        /// <remarks>
        ///     The method will return the current value of the attribute, even if the value has been modified after the page has been loaded. 
        ///     Note that the value of the following attributes will be returned even if there is no explicit attribute on the element: 
        ///     Attribute nameValue returned if not explicitly specified.
        /// </remarks>
        public string GetAttribute(string attributeName)
        {
            return _wrappedElement.GetAttribute(attributeName);
        }


        /// <summary>
        /// Gets the value of a JavaScript property of this element.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>The JavaScript property's current value. Returns a null if the value is not set or the property does not exist.
        ///</returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.
        /// </exception>
        public string GetProperty(string propertyName)
        {
            return _wrappedElement.GetProperty(propertyName);
        }

        /// <summary>
        ///     Gets the value of a CSS property of this element.
        /// </summary>
        /// <param name="propertyName">The name of the CSS property to get the value of.</param>
        /// <returns>The value of the specified CSS property.</returns>
        /// <remarks>
        ///     The value returned by the method is likely to be unpredictable in a cross-browser environment.  
        ///     Color values should be returned as hex strings. 
        ///     For example, a "background-color" property set as "green" in the HTML source, will return "#008000" for its value.
        /// </remarks>
        public string GetCssValue(string propertyName)
        {
            return _wrappedElement.GetCssValue(propertyName);
        }

        /// <summary>
        ///     Returns underlying web element wrapped by current <see cref="HtmlElement"/>
        /// </summary>
        public IWebElement WrappedElement
        {
            get
            {
                return _wrappedElement is IWrapsElement
                    ? (_wrappedElement as IWrapsElement).WrappedElement
                    : _wrappedElement;
            }
        }

        /// <summary>
        ///     Gets the tag name of this element.
        /// </summary>
        public string TagName
        {
            get { return _wrappedElement.TagName; }
        }

        /// <summary>
        ///     Gets the innerText of this element, without any leading or trailing whitespace, and with other whitespace collapsed.
        /// </summary>
        public string Text
        {
            get { return _wrappedElement.Text; }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this element is enabled.
        /// </summary>
        /// <remarks>
        ///     The property will generally return true for everything except explicitly disabled input elements.
        /// </remarks>
        public bool Enabled
        {
            get { return _wrappedElement.Enabled; }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this element is selected.
        /// </summary>
        /// <remarks>
        ///     This operation only applies to input elements such as checkboxes, options in a select element and radio buttons.
        /// </remarks>
        public bool Selected
        {
            get { return _wrappedElement.Selected; }
        }

        /// <summary>
        ///     Gets a <see cref="Point"/> object containing the coordinates of the upper-left corner of this element relative to the upper-left corner of the page.
        /// </summary>
        public Point Location
        {
            get { return _wrappedElement.Location; }
        }

        /// <summary>
        ///     Gets object containing the height and width of this element.
        /// </summary>
        public Size Size
        {
            get { return _wrappedElement.Size; }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this element is displayed.
        /// </summary>
        public bool Displayed
        {
            get { return _wrappedElement.IsPresent() && _wrappedElement.Displayed; }
        }

        /// <summary>
        ///     Replace existing text input with provided.
        /// </summary>
        /// <param name="text">Text to enter</param>
        public void EnterText(string text)
        {
            Clear();
            SendKeys(text);
        }

        /// <summary>
        ///     Clicks on a wrapped WebElement and creates new page object instance of given type.
        /// </summary>
        /// <typeparam name="TPage">
        ///     The type of page object being created.
        /// </typeparam>
        /// <returns>
        ///     New page object instance.
        /// </returns>
        public TPage Open<TPage>()
        {
            IWebDriver wd = WrappedDriver;

            Click();

            return PageObjectFactory.Create<TPage>(wd);
        }

        /// <summary>
        ///     Clicks on wrapped WebElement, waits until new window is opened and switches to it.
        /// </summary>
        /// <typeparam name="TPage">
        ///     Type of page object being created.
        /// </typeparam>
        /// <returns>
        ///     New page object instance.
        /// </returns>
        public TPage OpenInNewWindow<TPage>()
        {
            IWebDriver wd = WrappedDriver;

            wd.WaitUntilNewWindowOpened(Click);
            wd.SwitchToLastOpenedWindow();

            return PageObjectFactory.Create<TPage>(wd);
        }


    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace HtmlElements.Elements {

    /// <summary>
    ///     Models HTML select element and provides access to it's options
    /// </summary>
    public class HtmlSelect : HtmlControl, IList<HtmlSelectOption>
    {
        private readonly SelectElement _selectElement;

        /// <summary>
        ///     Initializes new instance of HTML element by calling base class constructor
        /// </summary>
        /// <param name="webElement">
        ///     WebElement wrapping WebDriver instance
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown when <paramref name="webElement" /> does not wrap WebDriver
        /// </exception>
        public HtmlSelect(IWebElement webElement) : base(webElement)
        {
            _selectElement = new SelectElement(webElement);
        }

        /// <summary>
        ///     Gets a value indicating whether select element supports multiple selections.
        /// </summary>
        public bool IsMultiple {
            get {
                var multiple = GetAttribute("multiple");

                return multiple != null && Boolean.Parse(multiple);
            }
        }

        /// <summary>
        ///     Gets the list of options for the select element.
        /// </summary>
        [FindsBy(How = How.TagName, Using = "option")]
        public IList<HtmlSelectOption> Options { get; private set; }

        /// <summary>
        ///     Gets all of the selected options within the select element
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "option[selected]")]
        public IList<HtmlSelectOption> SelectedOptions { get; private set; }

        /// <summary>
        ///     Gets the selected item within the select element.
        ///     If more than one item is selected this will return the first item.
        /// </summary>
        /// <exception cref="OpenQA.Selenium.NoSuchElementException">
        ///     Thrown if no option is selected.
        /// </exception>
        public HtmlSelectOption SelectedOption
        {
            get { 
                var selected = SelectedOptions.FirstOrDefault();

                if (selected == null)
                {
                    throw new NoSuchElementException("There are no selected options");
                }

                return selected;
            }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through select options.
        /// </summary>
        /// <returns>
        ///     An <see cref="System.Collections.IEnumerator"/> object that can be used to iterate through select options.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        ///     Gets select option at the specified index. 
        ///     Using setter will trigger <see cref="System.NotSupportedException"/>
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the select option to get.
        /// </param>
        /// <returns>
        ///     The select option at the specified index.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///     Thrown when attempted to set property value.
        /// </exception>
        public HtmlSelectOption this[int index] {
            get { return Options[index]; }
            set { throw ModificationAttemptException; }
        }

        /// <summary>
        ///     Not supported. Calling it will trigger <see cref="System.NotSupportedException"/>.
        /// </summary>
        /// <exception cref="System.NotSupportedException">
        ///     Thrown on any attempt to use method.
        /// </exception>
        public void RemoveAt(int index) {
            throw ModificationAttemptException;
        }

        /// <summary>
        ///     Not supported. Calling it will trigger <see cref="System.NotSupportedException"/>.
        /// </summary>
        /// <exception cref="System.NotSupportedException">
        ///     Thrown on any attempt to use method.
        /// </exception>
        public void Insert(int index, HtmlSelectOption item) {
            throw ModificationAttemptException;
        }

        /// <summary>
        ///     Determines the index of a specific option among other select options.
        /// </summary>
        /// <param name="item">
        ///     The options to locate in select element.
        /// </param>
        /// <returns>
        ///     The index of item if found; otherwise, -1.
        /// </returns>
        public int IndexOf(HtmlSelectOption item) {
            return Options.IndexOf(item);
        }

        /// <summary>
        ///     Always returns <value>true</value> since list is read-only
        /// </summary>
        public bool IsReadOnly {
            get { return true; }
        }

        /// <summary>
        ///     Gets the number of options contained in select element.
        /// </summary>
        public int Count {
            get { return Options.Count; }
        }

        /// <summary>
        ///     Not supported. Calling it will trigger <see cref="System.NotSupportedException"/>.
        /// </summary>
        /// <exception cref="System.NotSupportedException">
        ///     Thrown on any attempt to use method.
        /// </exception>
        public bool Remove(HtmlSelectOption item) {
            throw ModificationAttemptException;
        }

        /// <summary>
        ///     Copies select element options to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="System.Array"/> that is the destination of the copied select options. 
        ///     The <see cref="System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        ///     The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo(HtmlSelectOption[] array, int arrayIndex) {
            Options.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Determines whether the select element contains a specific option.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(HtmlSelectOption item) {
            return Options.Contains(item);
        }

        /// <summary>
        ///     Not supported. Calling it will trigger <see cref="System.NotSupportedException"/>.
        /// </summary>
        /// <exception cref="System.NotSupportedException">
        ///     Thrown on any attempt to use method.
        /// </exception>
        public void Add(HtmlSelectOption item) {
            throw ModificationAttemptException;
        }

        /// <summary>
        ///     Returns an enumerator that iterates through select options.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Collections.Generic.IEnumerator{T}"/> that can be used to iterate through select options.
        /// </returns>
        public IEnumerator<HtmlSelectOption> GetEnumerator() {
            return Options.GetEnumerator();
        }

        /// <summary>
        ///     Select all options by the text displayed.
        /// </summary>
        /// <param name="text">
        ///     The text of the option to be selected. If an exact match is not found, this method will perform a substring match.
        /// </param>
        public void SelectByText(string text) {
            _selectElement.SelectByText(text);
        }

        /// <summary>
        ///     Select an option by the value.
        /// </summary>
        /// <param name="value">
        ///     The value of the option to be selected.
        /// </param>
        public void SelectByValue(string value) {
            _selectElement.SelectByValue(value);
        }

        /// <summary>
        ///     Clear all selected entries. This is only valid when the SELECT supports multiple selections.
        /// </summary>
        /// <exception cref="OpenQA.Selenium.WebDriverException">
        ///     Thrown when attempting to deselect all options from a SELECT that does not support multiple selections.
        /// </exception>
        public void DeselectAll() {
            _selectElement.DeselectAll();
        }

        /// <summary>
        ///     Deselect the option by the text displayed.
        /// </summary>
        /// <param name="text">
        ///     The text of the option to be deselected.
        /// </param>
        public void DeselectByText(string text) {
            _selectElement.DeselectByText(text);
        }

        /// <summary>
        ///     Deselect the option having value matching the specified text.
        /// </summary>
        /// <param name="value">
        ///     The value of the option to deselect.
        /// </param>
        public void DeselectByValue(string value) {
            _selectElement.DeselectByValue(value);
        }

        private static NotSupportedException ModificationAttemptException
        {
            get { return new NotSupportedException("Attempted to modify read-only collection"); }
        }

    }

}
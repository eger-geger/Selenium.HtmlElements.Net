using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace HtmlElements.Elements {

    public class HtmlSelect : HtmlControl, IList<HtmlSelectOption> {

        public HtmlSelect(IWebElement wrapped) : base(wrapped) {}

        /// <summary>Gets a value indicating whether the parent element supports multiple selections.</summary>
        public bool IsMultiple {
            get {
                var multiple = GetAttribute("multiple");

                return multiple != null && Boolean.Parse(multiple);
            }
        }

        /// <summary>Gets the list of options for the select element.</summary>
        [FindsBy(How = How.TagName, Using = "option")]
        public IList<HtmlSelectOption> Options { get; private set; }

        /// <summary>Gets all of the selected options within the select element.</summary>
        [FindsBy(How = How.CssSelector, Using = "option[selected]")]
        public IList<HtmlSelectOption> AllSelectedOptions { get; private set; }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public HtmlSelectOption this[int index] {
            get { return Options[index]; }
            set { throw new ReadOnlyException(); }
        }

        public void RemoveAt(int index) {
            throw new ReadOnlyException();
        }

        public void Insert(int index, HtmlSelectOption item) {
            throw new ReadOnlyException();
        }

        public int IndexOf(HtmlSelectOption item) {
            return Options.IndexOf(item);
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public int Count {
            get { return Options.Count; }
        }

        public bool Remove(HtmlSelectOption item) {
            throw new ReadOnlyException();
        }

        public void CopyTo(HtmlSelectOption[] array, int arrayIndex) {
            Options.CopyTo(array, arrayIndex);
        }

        public bool Contains(HtmlSelectOption item) {
            return Options.Contains(item);
        }

        public void Add(HtmlSelectOption item) {
            throw new ReadOnlyException();
        }

        public IEnumerator<HtmlSelectOption> GetEnumerator() {
            return Options.GetEnumerator();
        }

        /// <summary cref="SelectElement.SelectByText" />
        public void SelectByText(string text) {
            new SelectElement(WrappedElement).SelectByText(text);
        }

        /// <summary cref="SelectElement.SelectByValue" />
        public void SelectByValue(string value) {
            new SelectElement(WrappedElement).SelectByValue(value);
        }

        /// <summary>De-select any selected option.</summary>
        public void DeselectAll() {
            new List<HtmlSelectOption>(AllSelectedOptions).ForEach(e => e.Selected = false);
        }

        /// <summary cref="SelectElement.DeselectByText" />
        public void DeselectByText(string text) {
            new SelectElement(WrappedElement).DeselectByText(text);
        }

        /// <summary cref="SelectElement.DeselectByValue" />
        public void DeselectByValue(string value) {
            new SelectElement(WrappedElement).DeselectByValue(value);
        }

    }

}
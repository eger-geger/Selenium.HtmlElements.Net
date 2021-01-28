using System.Collections.Generic;
using OpenQA.Selenium;

namespace HtmlElements.Elements
{
    /// <summary>
    ///     Models table DOM element and provides access to individual rows and columns.
    /// </summary>
    /// <seealso cref="HtmlElements.Elements.HtmlElement" />
    public class HtmlTable : HtmlElement
    {
        ///<summary>
        ///     Initializes new instance of HTML element by calling base class constructor.
        /// </summary>
        /// <param name="wrapped">
        ///     WebElement wrapping WebDriver instance.
        /// </param>
        public HtmlTable(IWebElement wrapped) : base(wrapped)
        {
        }

        /// <summary>
        ///     Get list of cells in a column with given index.
        /// </summary>
        /// <param name="index">
        ///     Table column index.
        /// </param>
        /// <returns>
        ///     List of column cells.
        /// </returns>
        public IList<HtmlElement> Column(int index)
        {
            return FindElements<HtmlElement>(By.CssSelector(string.Format("tr>*:nth-child({0})", index)));
        }

        /// <summary>
        ///     Get list of cells in a row with given index.
        /// </summary>
        /// <param name="index">
        ///     Table row index.
        /// </param>
        /// <returns>
        ///     List of row cells.
        /// </returns>
        public IList<HtmlElement> Row(int index)
        {
            return FindElements<HtmlElement>(By.CssSelector(string.Format("tr:nth-child({0})>*", index)));
        }
    }
}
using System.Collections.Generic;

using OpenQA.Selenium;

namespace HtmlElements.Elements {

    public class HtmlTable : HtmlElement {

        public HtmlTable(IWebElement wrapped) : base(wrapped) {}

        public IList<HtmlElement> Column(int index) {
            return ElementFactory.CreateElementList<HtmlElement>(
                RelativeLocator(By.CssSelector(string.Format("tr>*:nth-child({0})", index))));
        }

        public IList<HtmlElement> Row(int index) {
            return ElementFactory.CreateElementList<HtmlElement>(
                RelativeLocator(By.CssSelector(string.Format("tr:nth-child({0})>*", index))));
        }

    }

}
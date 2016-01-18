using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.Locators {

    public interface IElementProvider {

        IWebElement FindElement();

        ReadOnlyCollection<IWebElement> FindElements();

    }

}
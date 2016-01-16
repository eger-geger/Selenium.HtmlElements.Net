using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.Locators {

    public interface IElementLocator {

        IWebElement FindElement();

        ReadOnlyCollection<IWebElement> FindElements();

    }

}
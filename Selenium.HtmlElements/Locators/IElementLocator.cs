using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Locators {

    public interface IElementLocator {

        IWebElement FindElement();

        ReadOnlyCollection<IWebElement> FindElements();

    }

}
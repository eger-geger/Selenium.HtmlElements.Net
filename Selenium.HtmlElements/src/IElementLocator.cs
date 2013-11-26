using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Selenium.HtmlElements {

    public interface IElementLocator {

        IWebElement FindElement();

        ReadOnlyCollection<IWebElement> FindElements();

    }

}
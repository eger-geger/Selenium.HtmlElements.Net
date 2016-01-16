using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Elements {

    /// <summary>
    ///     Web element which is wrapping some <see cref="IWebDriver"/> instance and <see cref="IWebElement"/> instance and can execute JavaScript.
    /// </summary>
    public interface IHtmlElement : IWebElement, IWrapsElement, IWrapsDriver, IJavaScriptExecutor {

    }

}
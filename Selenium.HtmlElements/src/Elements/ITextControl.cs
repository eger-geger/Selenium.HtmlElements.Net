using OpenQA.Selenium;

namespace HtmlElements.Elements {

    /// <summary>
    ///     Marker interface describing element which main purpose is taking user text input.
    ///     It works in conjunction with <see cref="TextControlExtension"/>
    /// </summary>
    public interface ITextControl : IWebElement {

    }

    /// <summary>
    ///     Shared implementation of <see cref="ITextControl"/>
    /// </summary>
    public static class TextControlExtension {

        /// <summary>
        ///     Replace existing text input with provided
        /// </summary>
        /// <param name="self">WebElement in which text should be entered</param>
        /// <param name="text">Text to enter</param>
        public static void EnterText(this ITextControl self, string text) {
            self.Clear();
            self.SendKeys(text);
        }

    }

}
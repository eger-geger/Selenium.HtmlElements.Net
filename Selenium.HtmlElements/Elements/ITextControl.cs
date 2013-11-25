using OpenQA.Selenium;

namespace Selenium.HtmlElements.Elements {

    public interface ITextControl : IWebElement {

    }

    public static class TextControlExtension {

        public static void EnterText(this ITextControl self, string text) {
            self.Clear();
            self.SendKeys(text);
        }

    }

}
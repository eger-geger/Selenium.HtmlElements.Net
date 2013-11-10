using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using Selenium.HtmlElements.Elements;

using PageFactory = Selenium.HtmlElements.Factory.PageFactory;

#pragma warning disable 649

namespace Selenium.HtmlElements.Demo.Elements {

    internal class JaTabbedQuestionBox : HtmlElement {

        [FindsBy(How = How.CssSelector, Using = "#JA_submitQuestionButtonHolder input")]
        private readonly IHtmlElement _getAnswerButton;

        [FindsBy(How = How.Id, Using = "JA_questionBox")]
        private readonly IHtmlElement _questionText;

        public JaTabbedQuestionBox(IWebElement wrapped) : base(wrapped) {
            PageFactory.InitElementsIn(this, wrapped);
        }

        public void AskQuestion(string questionText) {
            _questionText.Clear();
            _questionText.SendKeys(questionText);
            _getAnswerButton.Click();
        }

    }

}
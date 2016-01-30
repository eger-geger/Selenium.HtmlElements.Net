using System;
using HtmlElements.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace HtmlElements.IntegrationTests.Pages
{
    public class PageBeta : HtmlFrame
    {
        [FindsBy(How = How.Id, Using = "login"), CacheLookup] 
        public HtmlInput LoginField;

        [FindsBy(How = How.Id, Using = "password"), CacheLookup] 
        public HtmlInput PasswordField;

        [FindsBy(How = How.Id, Using = "submit"), CacheLookup] 
        public HtmlElement SubmitBtn;

        public PageBeta(IWebElement webDriverOrWrapper) : base(webDriverOrWrapper)
        {
        }

        public void SignIn(String login, String password)
        {
            ExecuteInFrame(() =>
            {
                LoginField.EnterText(login);
                PasswordField.EnterText(password);
                SubmitBtn.Click();    
            });
        }
    }
}
using System;
using HtmlElements.Elements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

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

    public PageBeta(IWebElement webElement) : base(webElement)
    {
    }

    public void SignIn(string login, string password)
    {
      using (new FrameContextOverride(this))
      {
        LoginField.EnterText(login);
        PasswordField.EnterText(password);
        SubmitBtn.Click();
      }
    }
  }
}

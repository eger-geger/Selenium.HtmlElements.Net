using System;
using System.Collections.Generic;
using HtmlElements.Elements;
using HtmlElements.Locators;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace HtmlElements.Test.Locators
{
    public class ByFactoryTests
    {
        private static IEnumerable<ITestCaseData> StandardLocatorTestCases
        {
            get
            {
                var howToFactoryMapping = new Dictionary<How, Func<string, By>>
                {
                    {How.Id, By.Id},
                    {How.Name, By.Name},
                    {How.XPath, By.XPath},
                    {How.TagName, By.TagName},
                    {How.LinkText, By.LinkText},
                    {How.ClassName, By.ClassName},
                    {How.CssSelector, By.CssSelector},
                    {How.PartialLinkText, By.PartialLinkText},
                };

                foreach (var item in howToFactoryMapping)
                    yield return new TestCaseData(item.Key, "username").Returns(item.Value("username"));
            }
        }
        
        [TestCaseSource(nameof(StandardLocatorTestCases))]
        public By ShouldCreateLocator(How how, string locator)
        {
            return ByFactory.Create(how, locator);
        }

        [Test]
        public void ShouldThrowArgumentErrorWhenLocatorTypeIsNowKnown()
        {
            Assert.That(() => ByFactory.Create((How) 13, "username"), Throws.ArgumentException);
        }
        
        [Test]
        public void ShouldCreateLocatorFromMemberAttribute()
        {
            Assert.That(
                ByFactory.Create(new FindsByAttribute {How = How.Id, Using = "username"}),
                Is.EqualTo(By.Id("username"))
            );
        }
        
        [Test]
        public void ShouldCreateLocatorFromTypeAttribute()
        {
            Assert.That(
                ByFactory.Create(new ElementLocatorAttribute {How = How.Id, Using = "username"}),
                Is.EqualTo(By.Id("username"))
            );
        }
    }
}
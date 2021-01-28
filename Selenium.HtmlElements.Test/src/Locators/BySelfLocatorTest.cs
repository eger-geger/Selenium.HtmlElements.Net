using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HtmlElements.Elements;
using HtmlElements.Locators;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Locators
{
    [TestOf(typeof(ByDefaultLocator))]
    public class BySelfLocatorTest
    { 
        private const string WebElementName = "CustomWebElement";
        private static IEnumerable<TestCaseData> testCases = new List<TestCaseData>
        {
            new TestCaseData(typeof(IWebElement)).Returns(
                $"{WebElementName} of BySelfLocatorTest should either use FindsByAttribute or be derived from HtmlElement class and implement public member 'By Locator'"),
            new TestCaseData(typeof(WithEmptyLocator)).Returns(By.Id(WebElementName)),
            new TestCaseData(typeof(WithDefaultLocator)).Returns(By.Id("any"))
        };

        
        public class WithEmptyLocator : HtmlElement
        {
            public WithEmptyLocator(IWebElement webElement) : base(webElement)
            {
            }
        }

        public class WithDefaultLocator : HtmlElement
        {
            public override By DefaultLocator => By.Id("any");

            public WithDefaultLocator(IWebElement webElement) : base(webElement)
            {
            }
        }

        [TestCaseSource(nameof(testCases))]
        public object ItShouldUseDefaultElementLocator(Type elementType)
        {
            var mockMemberInfo = new Mock<PropertyInfo>();
            mockMemberInfo.SetupGet(x => x.Name).Returns(WebElementName);
            mockMemberInfo.SetupGet(x => x.DeclaringType).Returns(GetType());

            try
            {
                return ByDefaultLocator.Locate(mockMemberInfo.Object, elementType);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
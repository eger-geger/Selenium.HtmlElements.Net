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
    [TestOf(typeof(BySelf))]
    public class BySelfLocatorTest
    {
        private static IEnumerable<TestCaseData> testCases = new List<TestCaseData>
        {
            new TestCaseData(typeof(IWithLocator)).Returns(
                "CustomWebElement of BySelfLocatorTest should either use FindsByAttribute or be derived from HtmlElement class and implement public member 'By Locator'"),
            new TestCaseData(typeof(WithEmptyLocator)).Returns("WithEmptyLocator.Locator is not set"),
            new TestCaseData(typeof(WithLocatorProperty)).Returns(By.Id("any")),
            new TestCaseData(typeof(WithLocatorField)).Returns(By.Id("any"))
        };

        public interface IWithLocator
        {
            By Locator { get; }
        }

        public class WithEmptyLocator : HtmlElement
        {
            public By Locator;

            public WithEmptyLocator(IWebElement webElement) : base(webElement)
            {
            }
        }

        public class WithLocatorProperty : HtmlElement
        {
            public By Locator => By.Id("any");

            public WithLocatorProperty(IWebElement webElement) : base(webElement)
            {
            }
        }

        public class WithLocatorField : HtmlElement
        {
            public By Locator = By.Id("any");

            public WithLocatorField(IWebElement webElement) : base(webElement)
            {
            }
        }

        [TestCaseSource(nameof(testCases))]
        public object ItShouldUseDefaultElementLocator(Type elementType)
        {
            var webElementClassName = "CustomWebElement";
            var mockMemberInfo = new Mock<PropertyInfo>();
            mockMemberInfo.SetupGet(x => x.Name).Returns(webElementClassName);
            mockMemberInfo.SetupGet(x => x.DeclaringType).Returns(GetType());

            try
            {
                return BySelf.Locate(mockMemberInfo.Object, elementType);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
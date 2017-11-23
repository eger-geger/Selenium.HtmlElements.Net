using System;
using System.Collections.Generic;
using HtmlElements.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Extensions
{
    public class WaitForVisibleWebElementExtensionTests : AbstractWebElementExtensionsTestFixture
    {
        private static IEnumerable<object> WaitForVisibleTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new Func<IWebElement, TimeSpan, TimeSpan, String, IWebElement>(
                        (webElement, timeout, interval, message) => webElement.WaitForVisible(timeout, interval, message)
                        ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                    );

                yield return new TestCaseData(
                    new Func<IWebElement, TimeSpan, TimeSpan, String, IWebElement>(
                        (webElement, timeout, interval, message) => webElement.WaitForVisible(timeout, message)
                        ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                    );

                yield return new TestCaseData(
                    new Func<IWebElement, TimeSpan, TimeSpan, String, IWebElement>(
                        (webElement, timeout, interval, message) => webElement.WaitForVisible(message)
                        ),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                    );

                yield return new TestCaseData(
                    new Func<IWebElement, TimeSpan, TimeSpan, String, IWebElement>(
                        (webElement, timeout, interval, message) => webElement.WaitForVisible(timeout, interval, message)
                        ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    "Element did not became visible"
                    );
            }
        }

        [TestCaseSource("WaitForVisibleTestCases")]
        public void ShouldReturnOnceElementBecameVisible(
            Func<IWebElement, TimeSpan, TimeSpan, String, IWebElement> waitForVisible,
            TimeSpan timeout, TimeSpan pollingInterval, String errorMessage
            )
        {
            ElementMock.Setup(e => e.Displayed).Returns(false);

            ExecuteAsync(() => ElementMock.Setup(e => e.Displayed).Returns(true), timeout.Subtract(TimeSpan.FromSeconds(2)));

            Assert.That(waitForVisible(ElementMock.Object, timeout, pollingInterval, errorMessage), Is.SameAs(ElementMock.Object));
        }

        [TestCaseSource("WaitForVisibleTestCases")]
        public void ShouldHandleNoSuchElementException(
            Func<IWebElement, TimeSpan, TimeSpan, String, IWebElement> waitForVisible,
            TimeSpan timeout, TimeSpan pollingInterval, String errorMessage    
            )
        {
            ElementMock.Setup(e => e.Displayed).Throws<NoSuchElementException>();

            ExecuteAsync(() => ElementMock.Setup(e => e.Displayed).Returns(true), TimeSpan.FromSeconds(2));

            Assert.That(waitForVisible(ElementMock.Object, timeout, pollingInterval, errorMessage), Is.SameAs(ElementMock.Object));
        }

        [TestCaseSource("WaitForVisibleTestCases")]
        public void ShouldHandleStaleElementReferenceException(
            Func<IWebElement, TimeSpan, TimeSpan, String, IWebElement> waitForVisible,
            TimeSpan timeout, TimeSpan pollingInterval, String errorMessage
            )
        {
            ElementMock.Setup(e => e.Displayed).Throws<StaleElementReferenceException>();

            ExecuteAsync(() => ElementMock.Setup(e => e.Displayed).Returns(true), TimeSpan.FromSeconds(2));

            Assert.That(waitForVisible(ElementMock.Object, timeout, pollingInterval, errorMessage), Is.SameAs(ElementMock.Object));
        }

        [TestCaseSource("WaitForVisibleTestCases")]
        public void ShouldThrowWebDriverTimeoutExceptionWithGivenMessage(
            Func<IWebElement, TimeSpan, TimeSpan, String, IWebElement> waitForVisible, 
            TimeSpan timeout, TimeSpan pollingInterval, String errorMessage
            )
        {
            ElementMock.Setup(e => e.Displayed).Returns(false);

            Assert.That(
                () => waitForVisible(ElementMock.Object, timeout, pollingInterval, errorMessage),
                Throws.InstanceOf<WebDriverTimeoutException>().With.Message.Contains(errorMessage)
                );
        }
    }
}

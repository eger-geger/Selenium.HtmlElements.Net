using System;
using System.Collections.Generic;
using HtmlElements.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Extensions
{
    public class WaitUntilHiddenWebElementExtensionTests : AbstractWebElementExtensionsTestFixture
    {
        private static IEnumerable<object> WaitUntilHiddenTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new Action<IWebElement, TimeSpan, TimeSpan, string>(
                        (webElement, timeout, interval, message) =>
                            webElement.WaitUntilHidden(timeout, interval, message)
                    ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                );

                yield return new TestCaseData(
                    new Action<IWebElement, TimeSpan, TimeSpan, string>(
                        (webElement, timeout, interval, message) => webElement.WaitUntilHidden(timeout, message)
                    ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                );

                yield return new TestCaseData(
                    new Action<IWebElement, TimeSpan, TimeSpan, string>(
                        (webElement, timeout, interval, message) => webElement.WaitUntilHidden(message)
                    ),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                );

                yield return new TestCaseData(
                    new Action<IWebElement, TimeSpan, TimeSpan, string>(
                        (webElement, timeout, interval, message) =>
                            webElement.WaitUntilHidden(timeout, interval, message)
                    ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    "Element did not became visible"
                );
            }
        }

        [TestCaseSource(nameof(WaitUntilHiddenTestCases))]
        public void ShouldReturnOnceElementBecameHidden(
            Action<IWebElement, TimeSpan, TimeSpan, string> waitUntilHidden,
            TimeSpan timeout, TimeSpan pollingInterval, string errorMessage
        )
        {
            ElementMock.Setup(e => e.Displayed).Returns(true);

            ExecuteAsync(
                () => ElementMock.Setup(e => e.Displayed).Returns(false),
                timeout.Subtract(TimeSpan.FromSeconds(2))
            );

            waitUntilHidden(ElementMock.Object, timeout, pollingInterval, errorMessage);
        }

        [TestCaseSource(nameof(WaitUntilHiddenTestCases))]
        public void ShouldHandleNoSuchElementException(
            Action<IWebElement, TimeSpan, TimeSpan, string> waitUntilHidden,
            TimeSpan timeout, TimeSpan pollingInterval, string errorMessage
        )
        {
            ElementMock.Setup(e => e.Displayed).Returns(true);

            ExecuteAsync(() => ElementMock.Setup(e => e.Displayed).Throws<NoSuchElementException>(),
                TimeSpan.FromSeconds(2));

            waitUntilHidden(ElementMock.Object, timeout, pollingInterval, errorMessage);
        }

        [TestCaseSource(nameof(WaitUntilHiddenTestCases))]
        public void ShouldHandleStaleElementReferenceException(
            Action<IWebElement, TimeSpan, TimeSpan, string> waitUntilHidden,
            TimeSpan timeout, TimeSpan pollingInterval, string errorMessage
        )
        {
            ElementMock.Setup(e => e.Displayed).Throws<StaleElementReferenceException>();

            ExecuteAsync(() => ElementMock.Setup(e => e.Displayed).Returns(false), TimeSpan.FromSeconds(2));

            waitUntilHidden(ElementMock.Object, timeout, pollingInterval, errorMessage);
        }

        [TestCaseSource(nameof(WaitUntilHiddenTestCases))]
        public void ShouldThrowWebDriverTimeoutExceptionWithGivenMessage(
            Action<IWebElement, TimeSpan, TimeSpan, string> waitUntilHidden,
            TimeSpan timeout, TimeSpan pollingInterval, string errorMessage
        )
        {
            ElementMock.Setup(e => e.Displayed).Returns(true);

            Assert.That(
                () => waitUntilHidden(ElementMock.Object, timeout, pollingInterval, errorMessage),
                Throws.InstanceOf<WebDriverTimeoutException>().With.Message.Contains(errorMessage)
            );
        }
    }
}
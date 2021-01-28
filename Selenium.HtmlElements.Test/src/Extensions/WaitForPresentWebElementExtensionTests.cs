using System;
using System.Collections.Generic;
using System.Drawing;
using HtmlElements.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Extensions
{
    public class WaitForPresentWebElementExtensionTests : AbstractWebElementExtensionsTestFixture
    {
        private static IEnumerable<object> WaitForPresentTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new Func<IWebElement, TimeSpan, TimeSpan, string, IWebElement>(
                        (webElement, timeout, interval, message) =>
                            webElement.WaitForPresent(timeout, interval, message)
                    ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                );

                yield return new TestCaseData(
                    new Func<IWebElement, TimeSpan, TimeSpan, string, IWebElement>(
                        (webElement, timeout, interval, message) => webElement.WaitForPresent(timeout, message)
                    ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                );

                yield return new TestCaseData(
                    new Func<IWebElement, TimeSpan, TimeSpan, string, IWebElement>(
                        (webElement, timeout, interval, message) => webElement.WaitForPresent(message)
                    ),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(1),
                    "Element did not became visible"
                );

                yield return new TestCaseData(
                    new Func<IWebElement, TimeSpan, TimeSpan, string, IWebElement>(
                        (webElement, timeout, interval, message) =>
                            webElement.WaitForPresent(timeout, interval, message)
                    ),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    "Element did not became visible"
                );
            }
        }

        [TestCaseSource(nameof(WaitForPresentTestCases))]
        public void ShouldReturnOnceElementBecameAddedToDocument(
            Func<IWebElement, TimeSpan, TimeSpan, string, IWebElement> waitForPresent,
            TimeSpan timeout, TimeSpan pollingInterval, string errorMessage
        )
        {
            ElementMock.Setup(e => e.Size).Throws<NoSuchElementException>();

            ExecuteAsync(() => ElementMock.Setup(e => e.Size).Returns(new Size(1, 1)),
                timeout.Subtract(TimeSpan.FromSeconds(2)));

            Assert.That(waitForPresent(ElementMock.Object, timeout, pollingInterval, errorMessage),
                Is.SameAs(ElementMock.Object));
        }

        [TestCaseSource(nameof(WaitForPresentTestCases))]
        public void ShouldHandleNoSuchElementException(
            Func<IWebElement, TimeSpan, TimeSpan, string, IWebElement> waitForPresent,
            TimeSpan timeout, TimeSpan pollingInterval, string errorMessage
        )
        {
            ElementMock.Setup(e => e.Size).Throws<NoSuchElementException>();

            ExecuteAsync(() => ElementMock.Setup(e => e.Size).Returns(new Size(1, 1)), TimeSpan.FromSeconds(2));

            Assert.That(waitForPresent(ElementMock.Object, timeout, pollingInterval, errorMessage),
                Is.SameAs(ElementMock.Object));
        }

        [TestCaseSource(nameof(WaitForPresentTestCases))]
        public void ShouldHandleStaleElementReferenceException(
            Func<IWebElement, TimeSpan, TimeSpan, string, IWebElement> waitForPresent,
            TimeSpan timeout, TimeSpan pollingInterval, string errorMessage
        )
        {
            ElementMock.Setup(e => e.Size).Throws<StaleElementReferenceException>();

            ExecuteAsync(() => ElementMock.Setup(e => e.Size).Returns(new Size(1, 1)), TimeSpan.FromSeconds(2));

            Assert.That(waitForPresent(ElementMock.Object, timeout, pollingInterval, errorMessage),
                Is.SameAs(ElementMock.Object));
        }

        [TestCaseSource(nameof(WaitForPresentTestCases))]
        public void ShouldThrowWebDriverTimeoutExceptionWithGivenMessage(
            Func<IWebElement, TimeSpan, TimeSpan, string, IWebElement> waitForPresent,
            TimeSpan timeout, TimeSpan pollingInterval, string errorMessage
        )
        {
            ElementMock.Setup(e => e.Size).Throws<NoSuchElementException>();

            Assert.That(
                () => waitForPresent(ElementMock.Object, timeout, pollingInterval, errorMessage),
                Throws.InstanceOf<WebDriverTimeoutException>().With.Message.Contains(errorMessage)
            );
        }
    }
}
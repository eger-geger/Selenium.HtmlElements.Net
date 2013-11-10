using System.Drawing;

using NUnit.Framework;

using OpenQA.Selenium;

using Rhino.Mocks;

using Selenium.HtmlElements.Elements;
using Selenium.HtmlElements.Locators;
using Selenium.HtmlElements.Proxy;

namespace Selenium.HtmlElements.Test.Proxy {

    [TestFixture]
    public class WebElementCashTests : AssertionHelper {

        [SetUp]
        public void InitMockLocator() {
            _mockElementLocator = MockRepository.GenerateStub<IElementLocator>();
        }

        private readonly IHtmlElement _mockHtmlElement = MockRepository.GenerateStub<IHtmlElement>();
        private IElementLocator _mockElementLocator;

        [TestFixtureSetUp]
        public void InitMockElement() {
            _mockHtmlElement.Stub(e => e.Size).Return(new Size());
            _mockHtmlElement.Stub(e => e.WrappedDriver).Return(MockRepository.GenerateStub<IWebDriver>());
        }

        [Test]
        public void ElementShouldBeReloadedOnStaleReferenceExcpetion() {
            _mockElementLocator.Stub(l => l.FindElement()).Return(_mockHtmlElement);

            _mockElementLocator.Stub(l => l.FindElement())
                .Throw(new StaleElementReferenceException("HO!HO!HO!")).Repeat.Once()
                .Return(MockRepository.GenerateStub<IWebElement>()).Repeat.Once();

            var cash = new WebElementCash(_mockElementLocator);

            Expect(cash.Load().WrappedElement, Is.Not.Null);
        }

        [Test]
        public void LoadedElementShouldBeCashed() {
            _mockElementLocator.Stub(l => l.FindElement()).Return(_mockHtmlElement);

            var cash = new WebElementCash(_mockElementLocator);

            Expect(cash.Load().WrappedElement, Is.SameAs(cash.Load().WrappedElement));
        }

        [Test]
        public void ShouldLoadWebElement() {
            _mockElementLocator.Stub(l => l.FindElement()).Return(_mockHtmlElement);

            var cash = new WebElementCash(_mockElementLocator).Load();

            Expect(cash.WrappedElement, Is.Not.Null);
            Expect(cash.WrappedDriver, Is.Not.Null);
        }

        [Test, ExpectedException(typeof(NoSuchElementException))]
        public void ShouldThrowNoSuchElementException() {
            _mockElementLocator.Stub(l => l.FindElement()).Throw(new NoSuchElementException());

            new WebElementCash(_mockElementLocator).Load();
        }

    }

}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

using NUnit.Framework;

using OpenQA.Selenium;

using Rhino.Mocks;

using Selenium.HtmlElements.Proxy;
using Selenium.HtmlElements.Locators;

namespace Selenium.HtmlElements.Test.Proxy {

    [TestFixture]
    public class WebElementCashTests : AssertionHelper {

        private readonly IWebElement _mockHtmlElement = MockRepository.GenerateStub<IWebElement>();
        private readonly ReadOnlyCollection<IWebElement> _readOnlyList; 
        private IElementLocator _mockElementLocator;

        public WebElementCashTests() {
            _readOnlyList = new List<IWebElement> {_mockHtmlElement}.AsReadOnly();
        }

        [SetUp]
        public void InitMockLocator() {
            _mockElementLocator = MockRepository.GenerateStub<IElementLocator>();
        }
        
        [TestFixtureSetUp]
        public void InitMockElement() {
            _mockHtmlElement.Stub(e => e.Size).Return(new Size());
        }

        [Test]
        public void ElementShouldBeReloadedOnStaleReferenceExcpetion() {
            _mockElementLocator.Stub(l => l.FindElements()).Return(_readOnlyList);

            _mockElementLocator.Stub(l => l.FindElements())
                               .Throw(new StaleElementReferenceException("HO!HO!HO!")).Repeat.Once()
                               .Return(_readOnlyList).Repeat.Once();

            var loader = new ElementLoader(_mockElementLocator);

            Expect(loader.Load(true), Is.Not.Null);
        }

        [Test]
        public void LoadedElementShouldBeCashed() {
            _mockElementLocator.Stub(l => l.FindElements()).Return(_readOnlyList);

            var loader = new ElementLoader(_mockElementLocator);

            Expect(loader.Load(true), Is.SameAs(loader.Load(true)));
        }

        [Test]
        public void ShouldLoadWebElement() {
            _mockElementLocator.Stub(l => l.FindElements()).Return(_readOnlyList);

            var loader = new ElementLoader(_mockElementLocator);

            Expect(loader.Load(true), Is.Not.Null);
        }

        [Test, ExpectedException(typeof(NoSuchElementException))]
        public void ShouldThrowNoSuchElementException() {
            _mockElementLocator.Stub(l => l.FindElement()).Throw(new NoSuchElementException());

            new ElementLoader(_mockElementLocator).Load(true);
        }

    }

}
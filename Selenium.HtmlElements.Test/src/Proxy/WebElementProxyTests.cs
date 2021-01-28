using System;
using HtmlElements.LazyLoad;
using HtmlElements.Proxy;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Proxy
{
    public class WebElementProxyTests
    {
        private readonly ProxyFactory _proxyFactory = new ProxyFactory();

        private Mock<IWebElement> _elementMock;

        private Mock<ILoader<IWebElement>> _loaderMock;

        [SetUp]
        public void SetUpMocks()
        {
            _loaderMock = new Mock<ILoader<IWebElement>>();
            _elementMock = new Mock<IWebElement>();

            _loaderMock
                .Setup(loader => loader.Load())
                .Returns(_elementMock.Object);
        }

        [Test]
        public void ShouldHandleStaleElementReferenceException()
        {
            _elementMock.SetupSequence(e => e.Displayed)
                .Throws<StaleElementReferenceException>()
                .Returns(true);

            var elementProxy = _proxyFactory.CreateWebElementProxy(_loaderMock.Object);

            Assert.That(elementProxy.Displayed, Is.True);

            _loaderMock.Verify(loader => loader.Load(), Times.Exactly(2));
            _loaderMock.Verify(loader => loader.Reset(), Times.Once);
            _elementMock.Verify(e => e.Displayed, Times.Exactly(2));
        }

        [Test]
        public void ShouldHandleNotInCacheReferenceException()
        {
            _elementMock.SetupSequence(e => e.Displayed)
                .Throws(new InvalidOperationException(
                    "An unknown server-side error occurred while processing the command. Original error: Error while executing atom: Element does not exist in cache (status: 10)"))
                .Returns(true);

            var elementProxy = _proxyFactory.CreateWebElementProxy(_loaderMock.Object);

            Assert.That(elementProxy.Displayed, Is.True);

            _loaderMock.Verify(loader => loader.Load(), Times.Exactly(2));
            _loaderMock.Verify(loader => loader.Reset(), Times.Once);
            _elementMock.Verify(e => e.Displayed, Times.Exactly(2));
        }

        [Test]
        public void ShouldNotHandleOtherInvalidOperationException()
        {
            _elementMock.Setup(e => e.Click())
                .Throws<InvalidOperationException>();

            var elementProxy = _proxyFactory.CreateWebElementProxy(_loaderMock.Object);

            Assert.Throws<InvalidOperationException>(() => elementProxy.Click());

            _loaderMock.Verify(loader => loader.Load(), Times.Once);
            _loaderMock.Verify(loader => loader.Reset(), Times.Never);
        }

        [Test]
        public void ShouldGiveUpAfter5AttemptsToLoadElement()
        {
            _elementMock
                .Setup(e => e.Click())
                .Throws<StaleElementReferenceException>();

            var elementProxy = _proxyFactory.CreateWebElementProxy(_loaderMock.Object);

            Assert.Throws<StaleElementReferenceException>(() => elementProxy.Click());

            _loaderMock.Verify(loader => loader.Load(), Times.Exactly(5));
            _loaderMock.Verify(loader => loader.Reset(), Times.Exactly(5));
            _elementMock.Verify(e => e.Click(), Times.Exactly(5));
        }

        [Test]
        public void ShouldPreserveOriginalException()
        {
            _elementMock
                .Setup(e => e.Click())
                .Throws<NoSuchElementException>();

            var elementProxy = _proxyFactory.CreateWebElementProxy(_loaderMock.Object);

            Assert.Throws<NoSuchElementException>(() => elementProxy.Click());

            _loaderMock.Verify(loader => loader.Load(), Times.Once);
            _elementMock.Verify(e => e.Click(), Times.Once);
        }

        [Test]
        public void ShoulImplementWrapsElement()
        {
            var elementProxy = _proxyFactory.CreateWebElementProxy(_loaderMock.Object) as IWrapsElement;

            Assert.That(elementProxy, Is.Not.Null.And.InstanceOf<IWrapsElement>());
            Assert.That(elementProxy.WrappedElement, Is.EqualTo(_elementMock.Object));
        }
    }
}
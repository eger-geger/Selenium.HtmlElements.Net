using System;
using System.Collections.Generic;
using HtmlElements.LazyLoad;
using HtmlElements.Proxy;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Proxy
{
    public class ElementListProxyTests
    {
        private readonly ProxyFactory _proxyFactory = new ProxyFactory();

        private readonly List<IWebElement> _loadedElements = new List<IWebElement>
        {
            new Mock<IWebElement>().Object,
            new Mock<IWebElement>().Object
        };

        private Mock<ILoader<IList<IWebElement>>> _listLoaderMock;

        private IList<IWebElement> _elementListProxy;

        [SetUp]
        public void SetUpMocks()
        {
            _listLoaderMock = new Mock<ILoader<IList<IWebElement>>>();

            _elementListProxy = _proxyFactory.CreateListProxy(_listLoaderMock.Object);

            _listLoaderMock
                .Setup(loader => loader.Load())
                .Returns(_loadedElements);
        }

        [Test]
        public void ShouldLoadEelementsAndReturnElementCount()
        {
            Assert.That(_elementListProxy.Count, Is.EqualTo(2));

            _listLoaderMock.Verify(loader => loader.Load(), Times.Once);
        }

        [Test]
        public void ShouldLoadElementsAndReturnElementByIndex()
        {
            Assert.That(_elementListProxy[0], Is.EqualTo(_loadedElements[0]));

            _listLoaderMock.Verify(loader => loader.Load(), Times.Once);
        }

        [Test]
        public void ShouldLoadElementsAndReturnEnumerator()
        {
            Assert.That(_elementListProxy.GetEnumerator(), Is.EqualTo(_loadedElements.GetEnumerator()));

            _listLoaderMock.Verify(loader => loader.Load(), Times.Once);
        }

        [Test]
        public void ShouldLoadElementsAndTellWhetherListContainElement()
        {
            Assert.That(_elementListProxy.Contains(_loadedElements[0]), Is.True);

            _listLoaderMock.Verify(loader => loader.Load(), Times.Once);
        }

        [Test]
        public void ShouldLoadElementsAndCopyThemToArray()
        {
            IWebElement[] webElements = new IWebElement[2];

            _elementListProxy.CopyTo(webElements, 0);

            Assert.That(webElements, Is.EquivalentTo(_loadedElements));

            _listLoaderMock.Verify(loader => loader.Load(), Times.Once);
        }

        [Test]
        public void ShouldLoadElementsEndReturnElementIndex()
        {
            Assert.That(_elementListProxy.IndexOf(_loadedElements[0]), Is.EqualTo(0));

            _listLoaderMock.Verify(loader => loader.Load(), Times.Once);
        }

        [Test]
        public void ShouldBeReadOnly()
        {
            Assert.That(_elementListProxy.IsReadOnly, Is.True);

            _listLoaderMock.Verify(loader => loader.Load(), Times.Never);
        }

        [Test]
        public void ShouldNotAllowModification()
        {
            Assert.Throws<NotSupportedException>(() => _elementListProxy[3] = new Mock<IWebElement>().Object);
            Assert.Throws<NotSupportedException>(() => _elementListProxy.Add(new Mock<IWebElement>().Object));
            Assert.Throws<NotSupportedException>(() => _elementListProxy.Insert(0, new Mock<IWebElement>().Object));

            Assert.Throws<NotSupportedException>(() => _elementListProxy.Clear());
            Assert.Throws<NotSupportedException>(() => _elementListProxy.RemoveAt(0));
            Assert.Throws<NotSupportedException>(() => _elementListProxy.Remove(_loadedElements[0]));
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HtmlElements.Elements;
using HtmlElements.LazyLoad;
using HtmlElements.Proxy;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.LazyLoad
{
    public class DefaultLoaderFactoryTests
    {
        private LoaderFactory _loaderFactory;
        private Mock<IPageObjectFactory> _pageObjectFactoryMock;
        private Mock<IProxyFactory> _proxyFactoryMock;
        private Mock<IHtmlElement> _searchContextMock;
        private Mock<IWebDriver> _webDriverMock;
        
        [SetUp]
        public void SetUpMock()
        {
            _webDriverMock = new Mock<IWebDriver>();
            _searchContextMock = new Mock<IHtmlElement>();
            _pageObjectFactoryMock = new Mock<IPageObjectFactory>();
            _proxyFactoryMock = new Mock<IProxyFactory>();

            _loaderFactory = new LoaderFactory(_pageObjectFactoryMock.Object, _proxyFactoryMock.Object);

            _searchContextMock
                .Setup(ctx => ctx.WrappedDriver)
                .Returns(_webDriverMock.Object);

            _searchContextMock
                .Setup(ctx => ctx.FindElement(It.IsAny<By>()))
                .Returns(() => new Mock<IWebElement>().Object);

            _searchContextMock
                .Setup(ctx => ctx.FindElements(It.IsAny<By>()))
                .Returns(() => new ReadOnlyCollection<IWebElement>(
                    new List<IWebElement>
                    {
                        new Mock<IWebElement>().Object,
                        new Mock<IWebElement>().Object
                    }
                ));

            _pageObjectFactoryMock
                .Setup(f => f.Create<HtmlElement>(It.IsAny<ISearchContext>()))
                .Returns(() => new HtmlElement(_searchContextMock.Object));

            _proxyFactoryMock
                .Setup(f => f.CreateElementProxy(It.IsAny<ILoader<IWebElement>>()))
                .Returns(() => new Mock<IWebElement>().Object);
        }

        [Test]
        public void ShouldCreateTransparentElementLoader()
        {
            var loader = _loaderFactory.CreateElementLoader(
                _searchContextMock.Object, By.Id("transparent"), false
            );

            var firstElement = loader.Load();
            var secondElement = loader.Load();

            Assert.That(firstElement, Is.Not.SameAs(secondElement));

            _searchContextMock.Verify(
                ctx => ctx.FindElement(By.Id("transparent")), Times.Exactly(2)
            );
        }

        [Test]
        public void ShouldCreateCachedElementLoader()
        {
            var loader = _loaderFactory.CreateElementLoader(
                _searchContextMock.Object, By.Id("cached"), true
            );

            var firstElement = loader.Load();
            var secondElement = loader.Load();

            Assert.That(firstElement, Is.SameAs(secondElement));

            _searchContextMock.Verify(
                ctx => ctx.FindElement(By.Id("cached")), Times.Exactly(1)
            );
        }

        [Test]
        public void ShouldCreateTransaparentElementListLoader()
        {
            var loader = _loaderFactory.CreateElementListLoader(
                _searchContextMock.Object, By.Id("transparentList"), false
            );

            var firstList = loader.Load();
            var secondList = loader.Load();

            Assert.That(firstList, Is.Not.SameAs(secondList));

            _searchContextMock.Verify(
                ctx => ctx.FindElements(By.Id("transparentList")), Times.Exactly(2)
            );
        }

        [Test]
        public void ShouldCreateCachedElementListLoader()
        {
            var loader = _loaderFactory.CreateElementListLoader(
                _searchContextMock.Object, By.Id("cachedList"), true
            );

            var firstList = loader.Load();
            var secondList = loader.Load();

            Assert.That(firstList, Is.SameAs(secondList));

            _searchContextMock.Verify(
                ctx => ctx.FindElements(By.Id("cachedList")), Times.Exactly(1)
            );
        }

        [Test]
        public void ShouldCreateListLoaderInstanceOfGivenType()
        {
            var elementLoaderMock = new Mock<ILoader<ReadOnlyCollection<IWebElement>>>();

            Assert.That(
                _loaderFactory.CreateListLoader(typeof (HtmlElement), elementLoaderMock.Object, false),
                Is.Not.Null.And.InstanceOf<ILoader<IList<HtmlElement>>>()
            );

            Assert.That(
                _loaderFactory.CreateListLoader<HtmlElement>(elementLoaderMock.Object, false),
                Is.Not.Null.And.InstanceOf<ILoader<IList<HtmlElement>>>()
            );

            Assert.That(
                _loaderFactory.CreateListLoader(typeof (HtmlElement), _searchContextMock.Object, By.Id("any"), true),
                Is.Not.Null.And.InstanceOf<ILoader<IList<HtmlElement>>>()
            );

            Assert.That(
                _loaderFactory.CreateListLoader<HtmlElement>(_searchContextMock.Object, By.Id("any"), true),
                Is.Not.Null.And.InstanceOf<ILoader<IList<HtmlElement>>>()
            );
        }

        [Test]
        public void ShouldCreateTransparentListLoader()
        {
            var loader = _loaderFactory.CreateListLoader<HtmlElement>(
                _searchContextMock.Object, By.Id("transparentList"), false
            );

            var firstList = loader.Load();
            var secondList = loader.Load();

            Assert.That(firstList, Is.Not.SameAs(secondList));

            _searchContextMock.Verify(
                ctx => ctx.FindElements(By.Id("transparentList")), Times.Exactly(2)
            );

            _proxyFactoryMock.Verify(
                f => f.CreateElementProxy(It.IsNotNull<ILoader<IWebElement>>()), Times.Exactly(4)
            );

            _pageObjectFactoryMock.Verify(
                f => f.Create<HtmlElement>(It.IsNotNull<ISearchContext>()), Times.Exactly(4)
            );
        }

        [Test]
        public void ShouldCreateCachedListLoader()
        {
            var loader = _loaderFactory.CreateListLoader<HtmlElement>(
                _searchContextMock.Object, By.Id("cachedList"), true
            );

            var firstList = loader.Load();
            var secondList = loader.Load();

            Assert.That(firstList, Is.SameAs(secondList));

            _searchContextMock.Verify(
                ctx => ctx.FindElements(By.Id("cachedList")), Times.Exactly(1)
            );

            _proxyFactoryMock.Verify(
                f => f.CreateElementProxy(It.IsNotNull<ILoader<IWebElement>>()), Times.Exactly(2)
            );

            _pageObjectFactoryMock.Verify(
                f => f.Create<HtmlElement>(It.IsNotNull<ISearchContext>()), Times.Exactly(2)
            );
        }
    }
}
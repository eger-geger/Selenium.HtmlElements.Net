using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Test
{
    public class DefaultPageObjectFactoryTests
    {
        private readonly PageObjectFactory _pageObjectFactory = new PageObjectFactory();

        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Loose);

        [Test]
        public void ShouldCreatePageObjectAndInitializeElements()
        {
            var pageObjectA = _pageObjectFactory.Create<PageObjectA>(new Mock<IWebDriver>().Object);

            Assert.That(pageObjectA, Is.Not.Null);
        }

        [Test]
        public void ShouldCreatePageObjectWithCustomConstructor()
        {
            var webElement = new Mock<IWebElement>().Object;

            var pageObjectB = _pageObjectFactory.Create<PageObjectB>(webElement);

            Assert.That(pageObjectB, Is.Not.Null);
            Assert.That(pageObjectB.WebElement, Is.SameAs(webElement));
        }

        [Test]
        public void ShouldCreatePageObjectUsingSearchContextAndPageFactoryAsArgumetns()
        {
            var webElement = new Mock<IWebElement>().Object;

            var pageObjectC = _pageObjectFactory.Create<PageObjectC>(webElement);

            Assert.That(pageObjectC, Is.Not.Null);
            Assert.That(pageObjectC.WebElement, Is.SameAs(webElement));
            Assert.That(pageObjectC.PageFactory, Is.SameAs(_pageObjectFactory));
        }

        [Test]
        public void ShouldCreateWebElementUsingSearchContextLocator()
        {
            IWebElement wrappedWebElement = _mockRepository.OneOf<IWebElement>();

            Mock<ISearchContext> contextMock = new Mock<ISearchContext>();

            contextMock
                .Setup(ctx => ctx.FindElement(It.IsAny<By>()))
                .Returns(wrappedWebElement)
                .Verifiable();

            IWebElement webElement = _pageObjectFactory.CreateWebElement(contextMock.Object, By.Id("any"));

            Assert.That(webElement, Is.Not.Null.And.InstanceOf<IWrapsElement>());
            Assert.That((webElement as IWrapsElement).WrappedElement, Is.SameAs(wrappedWebElement));

            contextMock.Verify();
        }

        [Test]
        public void ShouldCreateWebElementListUsingSearchContextAndLocator()
        {
            var wrappedElementList = new ReadOnlyCollection<IWebElement>(new List<IWebElement>
            {
                _mockRepository.OneOf<IWebElement>(),
                _mockRepository.OneOf<IWebElement>(),
                _mockRepository.OneOf<IWebElement>()
            });

            Mock<ISearchContext> contextMock = new Mock<ISearchContext>();

            contextMock
                .Setup(ctx => ctx.FindElements(It.IsAny<By>()))
                .Returns(wrappedElementList)
                .Verifiable();

            ReadOnlyCollection<IWebElement> elementList = _pageObjectFactory.CreateWebElementList(contextMock.Object, By.ClassName("any"));

            Assert.That(elementList, Is.Not.Null);
            Assert.That(elementList.Count, Is.EqualTo(3));

            contextMock.Verify();
        }

        [Test]
        public void ShouldCreateCustomWebElementUsingSearchContextAndLocator()
        {
            IWebElement wrappedWebElement = _mockRepository.OneOf<IWebElement>();

            Mock<ISearchContext> contextMock = new Mock<ISearchContext>();

            contextMock
                .Setup(ctx => ctx.FindElement(It.IsAny<By>()))
                .Returns(wrappedWebElement)
                .Verifiable();

            PageObjectB pageObjectB = _pageObjectFactory.CreateWebElement<PageObjectB>(contextMock.Object, By.Id("any"));

            Assert.That(pageObjectB, Is.Not.Null);
            Assert.That(pageObjectB.WebElement, Is.Not.Null.And.InstanceOf<IWrapsElement>());
            Assert.That((pageObjectB.WebElement as IWrapsElement).WrappedElement, Is.EqualTo(wrappedWebElement));

            contextMock.Verify();
        }

        [Test]
        public void ShouldCreateListOfCustomWebElements()
        {
            var wrappedElementList = new ReadOnlyCollection<IWebElement>(new List<IWebElement>
            {
                _mockRepository.OneOf<IWebElement>(),
                _mockRepository.OneOf<IWebElement>(),
                _mockRepository.OneOf<IWebElement>()
            });

            Mock<ISearchContext> contextMock = new Mock<ISearchContext>();

            contextMock
                .Setup(ctx => ctx.FindElements(It.IsAny<By>()))
                .Returns(wrappedElementList)
                .Verifiable();

            IList<PageObjectA> elementList = _pageObjectFactory.CreateWebElementList<PageObjectA>(contextMock.Object, By.ClassName("any"));

            Assert.That(elementList, Is.Not.Null);
            Assert.That(elementList.Count, Is.EqualTo(3));

            contextMock.Verify();
        }

        public class PageObjectC
        {
            public PageObjectC(IWebElement webElement, IPageObjectFactory pageFactory)
            {
                WebElement = webElement;
                PageFactory = pageFactory;
            }

            public IPageObjectFactory PageFactory { get; private set; }
            public IWebElement WebElement { get; private set; }
        }

        public class PageObjectB
        {
            public PageObjectB(IWebElement webElement)
            {
                WebElement = webElement;
            }

            public IWebElement WebElement { get; private set; }
        }

        public class PageObjectA
        {

            private IWebElement _elementA;

            private IList<IWebElement> _elementListA;

        }
    }
}
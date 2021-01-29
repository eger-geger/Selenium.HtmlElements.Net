using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace HtmlElements.Test
{
    public class DefaultPageObjectFactoryTests
    {
        private readonly PageObjectFactory _pageObjectFactory = new();

        private readonly MockRepository _mockRepository = new(MockBehavior.Loose);

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
        public void ShouldCreatePageObjectUsingSearchContextAndPageFactoryAsArguments()
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
            var wrappedWebElement = _mockRepository.OneOf<IWebElement>();

            var contextMock = new Mock<ISearchContext>();

            contextMock
                .Setup(ctx => ctx.FindElement(It.IsAny<By>()))
                .Returns(wrappedWebElement)
                .Verifiable();

            var webElement = _pageObjectFactory.CreateWebElement(contextMock.Object, By.Id("any"));

            Assert.That(webElement, Is.Not.Null.And.InstanceOf<IWrapsElement>());
            Assert.That((webElement as IWrapsElement)?.WrappedElement, Is.SameAs(wrappedWebElement));

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

            var contextMock = new Mock<ISearchContext>();

            contextMock
                .Setup(ctx => ctx.FindElements(It.IsAny<By>()))
                .Returns(wrappedElementList)
                .Verifiable();

            var elementList =
                _pageObjectFactory.CreateWebElementList(contextMock.Object, By.ClassName("any"));

            Assert.That(elementList, Is.Not.Null);
            Assert.That(elementList.Count, Is.EqualTo(3));

            contextMock.Verify();
        }

        [Test]
        public void ShouldCreateCustomWebElementUsingSearchContextAndLocator()
        {
            var wrappedWebElement = _mockRepository.OneOf<IWebElement>();

            var contextMock = new Mock<ISearchContext>();

            contextMock
                .Setup(ctx => ctx.FindElement(It.IsAny<By>()))
                .Returns(wrappedWebElement)
                .Verifiable();

            var pageObjectB =
                _pageObjectFactory.CreateWebElement<PageObjectB>(contextMock.Object, By.Id("any"));

            Assert.That(pageObjectB, Is.Not.Null);
            Assert.That(pageObjectB.WebElement, Is.Not.Null.And.InstanceOf<IWrapsElement>());
            Assert.That((pageObjectB.WebElement as IWrapsElement)?.WrappedElement, Is.EqualTo(wrappedWebElement));

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

            var contextMock = new Mock<ISearchContext>();

            contextMock
                .Setup(ctx => ctx.FindElements(It.IsAny<By>()))
                .Returns(wrappedElementList)
                .Verifiable();

            var elementList =
                _pageObjectFactory.CreateWebElementList<PageObjectA>(contextMock.Object, By.ClassName("any"));

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
            [FindsBy(How = How.Id, Using = "any")] private IWebElement _elementA;

            [FindsBy(How = How.Id, Using = "any")] private IList<IWebElement> _elementListA;
        }
    }
}
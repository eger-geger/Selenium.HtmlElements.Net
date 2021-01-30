using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HtmlElements.Elements;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

// ReSharper disable CollectionNeverUpdated.Local
#pragma warning disable 649

#pragma warning disable 169

namespace HtmlElements.Test
{
    public class DefaultPageObjectFactoryTests
    {
        private readonly PageObjectFactory _pageObjectFactory = new();

        private readonly MockRepository _mockRepository = new(MockBehavior.Loose);

        [Test]
        public void ShouldCreatePageObjectAndInitializeElements()
        {
            var driverMock = _mockRepository.Create<IWebDriver>();

            driverMock
                .Setup(wd => wd.FindElement(By.Id("element-b")))
                .Returns(_mockRepository.OneOf<IWebElement>())
                .Verifiable();

            driverMock
                .Setup(wd => wd.FindElement(By.ClassName("element-a")))
                .Returns(_mockRepository.OneOf<IWebElement>())
                .Verifiable();

            driverMock
                .Setup(wd => wd.FindElements(By.ClassName("element-a")))
                .Returns(new List<IWebElement>
                {
                    _mockRepository.OneOf<IWebElement>(),
                    _mockRepository.OneOf<IWebElement>()
                }.AsReadOnly())
                .Verifiable();

            driverMock
                .Setup(wd => wd.FindElements(By.Name("list-element-b")))
                .Returns(new List<IWebElement>
                {
                    _mockRepository.OneOf<IWebElement>(),
                    _mockRepository.OneOf<IWebElement>()
                }.AsReadOnly())
                .Verifiable();

            var pageObjectA = _pageObjectFactory.Create<PageObjectA>(driverMock.Object);

            Assert.That(pageObjectA, Is.Not.Null);

            pageObjectA.Invalidate();

            driverMock.Verify();
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

            var elementList = _pageObjectFactory.CreateWebElementList(
                contextMock.Object,
                By.ClassName("any")
            );

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

        [Test]
        public void ShouldFailToInitializeMemberWithMultipleMemberAttributes()
        {
            Assert.That(
                () => _pageObjectFactory.Create<PageObjectD>(Mock.Of<ISearchContext>()),
                Throws.ArgumentException
            );
        }


        public class PageObjectD
        {
            [FindsBy(How = How.Id, Using = "any")]
            [FindsBy(How = How.Name, Using = "some")]
            private IWebElement _element;
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
            private ElementA _elementA;

            private IList<ElementA> _elementListA;

            [FindsBy(How = How.Id, Using = "element-b")]
            private IWebElement _elementB;

            [FindsBy(How = How.Name, Using = "list-element-b")]
            private IList<IWebElement> _elementListB;

            public void Invalidate()
            {
                _elementA.Click();
                _elementB.Click();
                _elementListA.FirstOrDefault()?.Click();
                _elementListB.FirstOrDefault()?.Click();
            }
        }

        [ElementLocator(How = How.ClassName, Using = "element-a")]
        public class ElementA : HtmlElement
        {
            public ElementA(IWebElement webElement) : base(webElement)
            {
            }
        }
    }
}
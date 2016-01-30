using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test
{
    public class DefaultPageObjectFactoryTests
    {
        private readonly PageObjectFactory _pageObjectFactory = new PageObjectFactory();

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
        }
    }
}
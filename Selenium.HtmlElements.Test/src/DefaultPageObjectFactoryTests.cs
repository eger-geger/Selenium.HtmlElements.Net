using System.Collections.Generic;
using HtmlElements.Elements;
using HtmlElements.Proxy;
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
            var page = _pageObjectFactory.Create<PageObjectA>(new Mock<ISearchContext>().Object);

            Assert.That(page, Is.Not.Null);
            Assert.That(page.ElementA, Is.Not.Null.And.InstanceOf<HtmlElement>());
            Assert.That(page.GetElementB(), Is.Not.Null.And.InstanceOf<HtmlElement>());
            Assert.That(page.ElementListA, Is.Not.Null.And.InstanceOf<ElementListProxy<HtmlImage>>());
        }

        [Test]
        public void ShouldCreatePageObjectWithCustomConstructor()
        {
            IWebElement webElement = new Mock<IWebElement>().Object;

            var pageObjectB = _pageObjectFactory.Create<PageObjectB>(webElement);

            Assert.That(pageObjectB, Is.Not.Null);
            Assert.That(pageObjectB.WebElement, Is.SameAs(webElement));
        }

        [Test]
        public void ShouldCreatePageObjectUsingSearchContextAndPageFactoryAsArgumetns()
        {
            IWebElement webElement = new Mock<IWebElement>().Object;

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
            private IHtmlElement _elementB;

            public IWebElement ElementA { get; private set; }

            public IList<HtmlImage> ElementListA { get; private set; } 

            public IHtmlElement GetElementB()
            {
                return _elementB;
            }
        }

    }
}
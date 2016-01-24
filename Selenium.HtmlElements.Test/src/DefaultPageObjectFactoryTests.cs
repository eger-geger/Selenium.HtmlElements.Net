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
            var pageObject = _pageObjectFactory.Create<PageObjectB>(new Mock<IWebElement>().Object);

            Assert.That(pageObject, Is.Not.Null);
        }

        public class PageObjectB
        {
            public PageObjectB(IWebElement webElement){}
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
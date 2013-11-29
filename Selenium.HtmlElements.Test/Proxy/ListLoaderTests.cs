using System.Collections.Generic;
using System.Collections.ObjectModel;

using HtmlElements.Locators;
using HtmlElements.Proxy;

using NUnit.Framework;

using OpenQA.Selenium;

using Rhino.Mocks;

namespace HtmlElements.Test.Proxy {

    [TestFixture]
    public class ListLoaderTests : AssertionHelper {

        private readonly ReadOnlyCollection<IWebElement> _elementList = new List<IWebElement> {
            MockRepository.GenerateMock<IWebElement>()
        }.AsReadOnly();

        [Test]
        public void ShouldLoadElementList() {
            var locator = MockRepository.GenerateMock<IElementLocator>();
            locator.Stub(l => l.FindElements()).Return(_elementList);

            var loader = new ElementListLoader(typeof(IWebElement), locator);

            Expect(loader.Load(true), Is.Not.Null);
            Expect(loader.Load(false), Is.Not.Null);
        }

        [Test]
        public void ShouldIgnoreStaleElementReferenceException() {
            var locator = MockRepository.GenerateMock<IElementLocator>();
            locator.Stub(l => l.FindElements())
                .Throw(new StaleElementReferenceException("HOHOHO")).Repeat.Once();

            locator.Stub(l => l.FindElements()).Return(_elementList);

            var loader = new ElementListLoader(typeof(IWebElement), locator);

            Expect(loader.Load(true), Is.Not.Null);
        }

        [Test]
        public void ShouldCacheElementList() {
            var locator = MockRepository.GenerateMock<IElementLocator>();
            locator.Stub(l => l.FindElements()).Return(_elementList).Repeat.Once();
            locator.Stub(l => l.FindElements()).Return(new List<IWebElement>().AsReadOnly());

            var loader = new ElementListLoader(typeof(IWebElement), locator);

            Expect(loader.Load(true), Is.SameAs(loader.Load(true)));
        }
    }

}
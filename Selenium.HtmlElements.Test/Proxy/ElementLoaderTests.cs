using HtmlElements.Locators;
using HtmlElements.Proxy;

using NUnit.Framework;

using OpenQA.Selenium;

using Rhino.Mocks;

namespace HtmlElements.Test.Proxy
{
    [TestFixture]
    public class ElementLoaderTests : AssertionHelper {

        private readonly IWebElement _webElement = MockRepository.GenerateMock<IWebElement>();

        [Test]
        public void ShouldLoadElement(){
            var locator = MockRepository.GenerateMock<IElementLocator>();
            var loader = new ElementLoader(locator, false);
            
            locator.Stub(l => l.FindElement()).Return(_webElement);

            Expect(loader.Load(), Is.Not.Null);
            Expect(loader.Load(), Is.Not.Null);
        }

        [Test]
        public void ShouldIgnoreStaleElementReferenceException(){
            var locator = MockRepository.GenerateMock<IElementLocator>();
            var loader = new ElementLoader(locator, true);

            locator.Stub(l => l.FindElement())
                .Throw(new StaleElementReferenceException("HOHOHO")).Repeat.Once();
            locator.Stub(l => l.FindElement()).Return(_webElement);

            Expect(loader.Load(), Is.Not.Null);
        }

        [Test]
        public void ShouldCacheElement(){
            var locator = MockRepository.GenerateMock<IElementLocator>();
            var loader = new ElementLoader(locator, true);

            locator.Stub(l => l.FindElement()).Return(_webElement).Repeat.Once();
            locator.Stub(l => l.FindElement()).Return(MockRepository.GenerateMock<IWebElement>());

            Expect(loader.Load(), Is.SameAs(loader.Load()));
            Expect(loader.Load(), Is.SameAs(_webElement));
        }


    }
}

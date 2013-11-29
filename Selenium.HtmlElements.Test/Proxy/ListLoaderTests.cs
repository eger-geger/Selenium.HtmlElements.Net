using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using NUnit.Framework;

using OpenQA.Selenium;

using Rhino.Mocks;

using Selenium.HtmlElements.Locators;
using Selenium.HtmlElements.Proxy;

namespace Selenium.HtmlElements.Test.Proxy
{
    [TestFixture]
    public class ListLoaderTests : AssertionHelper {

        private readonly ReadOnlyCollection<IWebElement> _elementList = new List<IWebElement> {
            MockRepository.GenerateMock<IWebElement>()
        }.AsReadOnly();

        [Test]
        public void ShouldLoadElement() {
            var locator = MockRepository.GenerateMock<IElementLocator>();
            locator.Stub(l => l.FindElements()).Return(_elementList);

            var loader = new ElementListLoader(typeof(IWebElement), locator);

            Expect(loader.Load(true), Is.Not.Null);
            Expect(loader.Load(false), Is.Not.Null);
        }
    }
}

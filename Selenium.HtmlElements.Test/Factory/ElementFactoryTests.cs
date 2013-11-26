using System;
using System.Collections.Generic;

using NUnit.Framework;

using OpenQA.Selenium;

using Rhino.Mocks;

using Selenium.HtmlElements.Elements;

namespace Selenium.HtmlElements.Test.Factory {

    [TestFixture]
    public class ElementFactoryTests : AssertionHelper {

        private readonly IElementLocator _mockLocator = MockRepository.GenerateStub<IElementLocator>();

        public IEnumerable<TestCaseData> ElementsToCreate() {
            yield return new TestCaseData(typeof(IWebElement));
            yield return new TestCaseData(typeof(IHtmlElement));
            yield return new TestCaseData(typeof(HtmlElement));
            yield return new TestCaseData(typeof(HtmlCheckBox));

            yield return new TestCaseData(typeof(IList<IWebElement>));
            yield return new TestCaseData(typeof(IList<IHtmlElement>));
            yield return new TestCaseData(typeof(IList<HtmlElement>));
            yield return new TestCaseData(typeof(IList<HtmlCheckBox>));
        }

        [TestCaseSource("ElementsToCreate")]
        public void ShouldCreateElement(Type type) {
            var created = ElementFactory.Create(type, _mockLocator);

            Expect(created, Is.Not.Null);
            Expect(created, Is.AssignableTo(type));
        }

    }

}
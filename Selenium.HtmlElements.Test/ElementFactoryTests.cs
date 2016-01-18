using System;
using System.Collections.Generic;
using HtmlElements.Elements;
using HtmlElements.Locators;
using NUnit.Framework;

using OpenQA.Selenium;

using Rhino.Mocks;

namespace HtmlElements.Test {

    [TestFixture]
    public class ElementFactoryTests : AssertionHelper {

        private readonly IElementProvider _mockProvider = MockRepository.GenerateStub<IElementProvider>();

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
            var created = ElementFactory.Create(type, _mockProvider);

            Expect(created, Is.Not.Null);
            Expect(created, Is.AssignableTo(type));
        }

    }

}
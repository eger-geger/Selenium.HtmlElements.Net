using System;
using System.Collections.Generic;

using HtmlElements.Elements;

using NUnit.Framework;

using OpenQA.Selenium;

using Rhino.Mocks;

namespace HtmlElements.Test {

    [TestFixture]
    public class ElementActivatorTests : AssertionHelper {

        private class TestPage {

            public IList<IHtmlElement> _elementList;

            public IWebElement _webElement;

            public IList<IWebElement> ElementList { get; private set; }

            public IWebElement WebElement { get; private set; }

            public IWebElement this[int index] {
                get { return null; }
                set { throw new InvalidOperationException("Should Never be called"); }
            }

            public IList<IWebElement> ReadOnlyList {
                get { return null; }
            }

        }

        private readonly IWebDriver _mockWebDriver = MockRepository.GenerateStub<IWebDriver>();
        private readonly ISearchContext _mockSearchContext = MockRepository.GenerateStub<ISearchContext>();
        private readonly IWebElement _mockWebElement = MockRepository.GenerateStub<IWebElement>();

        public IEnumerable<TestCaseData> ConstructorArguments {
            get {
                yield return new TestCaseData(typeof(Wrapper<IWebDriver>),
                    _mockWebDriver, new Wrapper<IWebDriver>(_mockWebDriver));
                yield return new TestCaseData(typeof(Wrapper<ISearchContext>),
                    _mockSearchContext, new Wrapper<ISearchContext>(_mockSearchContext));
                yield return new TestCaseData(typeof(Wrapper<ISearchContext>),
                    _mockWebDriver, new Wrapper<ISearchContext>(_mockWebDriver));
                yield return new TestCaseData(typeof(Wrapper<ISearchContext>),
                    _mockWebElement, new Wrapper<ISearchContext>(_mockWebElement));
                yield return new TestCaseData(typeof(Wrapper<IWebElement>),
                    _mockWebElement, new Wrapper<IWebElement>(_mockWebElement));
            }
        }

        [TestCaseSource("ConstructorArguments")]
        public void ShouldCreateInstances(Type type, ISearchContext context, object expected) {
            var target = ElementActivator.Activate(type, context);

            Expect(target, Is.Not.Null);
            Expect(target, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldIgnoreElements() {
            var page = ElementActivator.Activate<TestPage>(_mockSearchContext);

            Expect(page[0], Is.Null);
            Expect(page.ReadOnlyList, Is.Null);
        }

        [Test]
        public void ShouldInitElements() {
            var page = ElementActivator.Activate<TestPage>(_mockSearchContext);

            Expect(page.ElementList, Is.Not.Null);
            Expect(page.WebElement, Is.Not.Null);
            Expect(page._elementList, Is.Not.Null);
            Expect(page._webElement, Is.Not.Null);
        }

    }

    internal class Wrapper<T> {

        public Wrapper(T wrapped) {
            Wrapped = wrapped;
        }

        public T Wrapped { get; private set; }

        protected bool Equals(Wrapper<T> other) {
            return Equals(Wrapped, other.Wrapped);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Wrapper<T>) obj);
        }

        public override int GetHashCode() {
            return (Wrapped != null ? Wrapped.GetHashCode() : 0);
        }

    }

}
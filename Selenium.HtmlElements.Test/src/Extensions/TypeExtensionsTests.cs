﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using HtmlElements.Elements;
using HtmlElements.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
#pragma warning disable 649
#pragma warning disable 169

namespace HtmlElements.Test.Extensions
{
    public class TypeExtensionsTests
    {
        [TestCase(null, ExpectedResult = false)]
        [TestCase(typeof(object), ExpectedResult = false)]
        [TestCase(typeof(IWebDriver), ExpectedResult = false)]
        [TestCase(typeof(ISearchContext), ExpectedResult = false)]
        [TestCase(typeof(IHtmlElement), ExpectedResult = true)]
        [TestCase(typeof(IWebElement), ExpectedResult = true)]
        [TestCase(typeof(HtmlElement), ExpectedResult = true)]
        [TestCase(typeof(WebElementImpl), ExpectedResult = true)]
        public bool ShouldDetermineWeatherTypeIsDerivedFromWebElement(Type type)
        {
            return type.IsWebElement();
        }

        [TestCase(typeof(IList<IWebElement>), typeof(IWebElement), ExpectedResult = true)]
        [TestCase(typeof(IList<WebElementImpl>), typeof(WebElementImpl), ExpectedResult = true)]
        [TestCase(typeof(IList<IHtmlElement>), typeof(IHtmlElement), ExpectedResult = true)]
        [TestCase(typeof(IList<HtmlElement>), typeof(HtmlElement), ExpectedResult = true)]
        [TestCase(typeof(IList<object>), null, ExpectedResult = false)]
        [TestCase(typeof(IList<IWebDriver>), null, ExpectedResult = false)]
        [TestCase(typeof(IList<ISearchContext>), null, ExpectedResult = false)]
        [TestCase(typeof(IEnumerable<IWebElement>), null, ExpectedResult = false)]
        [TestCase(typeof(ICollection<IWebElement>), null, ExpectedResult = false)]
        [TestCase(typeof(List<IWebElement>), null, ExpectedResult = false)]
        public bool ShouldDetermineWeatherTypeDescribedListOfWebElements(Type listType, Type listElementType)
        {
            bool result = listType.IsWebElementList(out var elementType);
            Assert.That(elementType, Is.EqualTo(listElementType));
            return result;
        }

        [Test]
        public void ShouldFindAllPropertiesMatchingConstraints()
        {
            IList<string> properties = typeof(PageObjecB)
                .GetOwnAndInheritedProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(property => property.Name)
                .ToList();

            Assert.That(properties, Is.EquivalentTo(new[] {"ElemetnB", "ElementListC"}));
        }

        [Test]
        public void ShouldFindAllFieldsMatchingConstraints()
        {
            IList<string> fields = typeof(PageObjecB)
                .GetOwnAndInheritedFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Select(property => property.Name)
                .ToList();

            Assert.That(fields, Is.EquivalentTo(new[] {"_elementA", "_elementListA", "_elementListB", "_elementC"}));
        }

        private class PageObjectA
        {
            private IWebElement _elementC;

            public IWebElement _elementA;

            public IList<IWebElement> _elementListA;

            public IWebElement ElemetnB { get; set; }
        }

        private class PageObjecB : PageObjectA
        {
            public IList<IHtmlElement> _elementListB;

            public IList<HtmlElement> ElementListC { get; set; }
        }

        private class WebElementImpl : IWebElement
        {
            public IWebElement FindElement(By @by)
            {
                throw new NotImplementedException();
            }

            public ReadOnlyCollection<IWebElement> FindElements(By @by)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public void SendKeys(string text)
            {
                throw new NotImplementedException();
            }

            public void Submit()
            {
                throw new NotImplementedException();
            }

            public void Click()
            {
                throw new NotImplementedException();
            }

            public string GetAttribute(string attributeName)
            {
                throw new NotImplementedException();
            }

            public string GetDomAttribute(string attributeName)
            {
                throw new NotImplementedException();
            }

            public string GetDomProperty(string propertyName)
            {
                throw new NotImplementedException();
            }

            public string GetCssValue(string propertyName)
            {
                throw new NotImplementedException();
            }

            public ISearchContext GetShadowRoot()
            {
                throw new NotImplementedException();
            }

            public string GetProperty(string propertyName)
            {
                throw new NotImplementedException();
            }

            public string TagName { get; set; }

            public string Text { get; set; }

            public bool Enabled { get; set; }

            public bool Selected { get; set; }

            public Point Location { get; set; }

            public Size Size { get; set; }

            public bool Displayed { get; set; }
        }
    }
}
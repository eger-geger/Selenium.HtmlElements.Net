using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using HtmlElements.Elements;
using HtmlElements.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Extensions
{
    public class TypeExtensionsTests
    {
        [TestCase(null, Result = false)]
        [TestCase(typeof(Object), Result = false)]
        [TestCase(typeof(IWebDriver), Result = false)]
        [TestCase(typeof(ISearchContext), Result = false)]
        [TestCase(typeof(IHtmlElement), Result = true)]
        [TestCase(typeof(IWebElement), Result = true)]
        [TestCase(typeof(HtmlElement), Result = true)]
        [TestCase(typeof(WebElementImpl), Result = true)]
        public Boolean ShouldDetermineWeatherTypeIsDerivedFromWebElement(Type type)
        {
            return type.IsWebElement();
        }

        [TestCase(typeof(IList<IWebElement>), Result = true)]
        [TestCase(typeof(IList<WebElementImpl>), Result = true)]
        [TestCase(typeof(IList<IHtmlElement>), Result = true)]
        [TestCase(typeof(IList<HtmlElement>), Result = true)]
        [TestCase(typeof(IList<Object>), Result = false)]
        [TestCase(typeof(IList<IWebDriver>), Result = false)]
        [TestCase(typeof(IList<ISearchContext>), Result = false)]
        [TestCase(typeof(IEnumerable<IWebElement>), Result = false)]
        [TestCase(typeof(ICollection<IWebElement>), Result = false)]
        [TestCase(typeof(List<IWebElement>), Result = false)]
        public Boolean ShouldDetermineWeatherTypeDescribedListOfWebElements(Type type)
        {
            return type.IsWebElementList();
        }

        [Test]
        public void ShouldFindAllPropertiesMatchingConstraints()
        {
            IList<String> properties = typeof (PageObjecB)
                .GetOwnAndInheritedProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(property => property.Name)
                .ToList();

            Assert.That(properties, Is.EquivalentTo(new[] { "ElemetnB", "ElementListC" }));
        }

        [Test]
        public void ShouldFindAllFieldsMatchingConstraints()
        {
            IList<String> fields = typeof(PageObjecB)
                .GetOwnAndInheritedFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Select(property => property.Name)
                .ToList();

            Assert.That(fields, Is.EquivalentTo(new[] { "_elementA", "_elementListA", "_elementListB", "_elementC" }));
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

            public string GetCssValue(string propertyName)
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

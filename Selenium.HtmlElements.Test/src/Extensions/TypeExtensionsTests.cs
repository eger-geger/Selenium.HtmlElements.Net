using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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

            public string TagName { get; }
            public string Text { get; }
            public bool Enabled { get; }
            public bool Selected { get; }
            public Point Location { get; }
            public Size Size { get; }
            public bool Displayed { get; }
        }

    }
}

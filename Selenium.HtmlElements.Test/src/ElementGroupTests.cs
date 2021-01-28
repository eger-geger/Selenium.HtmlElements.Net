using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test
{
    public class ElementGroupTests
    {
        [Test]
        public void ShouldLoadAllElementsBelongingToGroup()
        {
            var pageObjectB = new PageObjectB();

            Assert.That(new ElementGroup("alpha").GetElements(pageObjectB), Is.EquivalentTo(
                new Dictionary<string, IWebElement>
                {
                    {
                        "ElementA", pageObjectB.ElementA
                    },
                    {
                        "ElementB", pageObjectB.ElementB
                    },
                    {
                        "ElementD", pageObjectB.ElementD
                    }
                }));

            Assert.That(new ElementGroup("beta").GetElements(pageObjectB), Is.EquivalentTo(
                new Dictionary<string, IWebElement>
                {
                    {
                        "ElementA", pageObjectB.ElementA
                    },
                    {
                        "ElementE", pageObjectB.ElementE
                    }
                }));
        }

        [Test]
        public void ShouldIncludeElementsFromMultipleGroups()
        {
            var pageObjectB = new PageObjectB();

            Assert.That(new ElementGroup("alpha", "beta").GetElements(pageObjectB), Is.EquivalentTo(
                new Dictionary<string, IWebElement>
                {
                    {
                        "ElementA", pageObjectB.ElementA
                    },
                    {
                        "ElementB", pageObjectB.ElementB
                    },
                    {
                        "ElementD", pageObjectB.ElementD
                    },
                    {
                        "ElementE", pageObjectB.ElementE
                    }
                }));
        }

        private static IWebElement CreateWebElement()
        {
            return new Mock<IWebElement>().Object;
        }

        public class PageObjectA
        {
            [ElementGroup("alpha", "beta")] public readonly IWebElement ElementA = CreateWebElement();

            [ElementGroup("alpha")] public readonly IWebElement ElementB = CreateWebElement();

            public readonly IWebElement ElementC = CreateWebElement();
        }

        public class PageObjectB : PageObjectA
        {
            private IWebElement _elementD = CreateWebElement();

            public PageObjectB()
            {
                ElementE = CreateWebElement();
            }

            [ElementGroup("alpha")]
            public IWebElement ElementD
            {
                get => _elementD;
                set => _elementD = value;
            }

            [ElementGroup("beta")]
            public IWebElement ElementE { get; private set; }
        }
    }
}
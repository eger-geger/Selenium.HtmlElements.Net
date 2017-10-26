using System.Drawing;
using HtmlElements.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Extensions
{
    public class IsPresentWebElementExtensionTests : AbstractWebElementExtensionsTestFixture
    {
        [Test]
        public void IsPresentShouldYieldTrueWhenElementExist()
        {
            ElementMock.Setup(e => e.Size).Returns(new Size(100, 100));

            Assert.True(ElementMock.Object.IsPresent());
        }

        [Test]
        public void IsPresentShouldYieldFalseWhenElementDoesNotExist()
        {
            ElementMock.Setup(e => e.Size).Throws<NoSuchElementException>();

            Assert.True(ElementMock.Object.IsPresent());
        }
    }
}

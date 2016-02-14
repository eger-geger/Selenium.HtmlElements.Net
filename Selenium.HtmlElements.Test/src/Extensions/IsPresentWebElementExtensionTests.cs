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

            Assert.That(ElementMock.Object.IsPresent(), Is.True);
        }

        [Test]
        public void IsPresentShouldYieldFalseWhenElementDoesNotExist()
        {
            ElementMock.Setup(e => e.Size).Throws<NoSuchElementException>();

            Assert.That(ElementMock.Object.IsPresent(), Is.False);
        }
    }
}

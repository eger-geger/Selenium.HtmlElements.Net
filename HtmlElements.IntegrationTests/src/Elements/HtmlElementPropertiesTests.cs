using HtmlElements.Elements;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.IntegrationTests.Elements
{
    public class HtmlElementPropertiesTests : IntegrationTestFixture
    {
        HtmlElement elementList;
        HtmlElement hamlet;


        [SetUp]
        public void SetUp()
        {
            elementList = PageAlpha.FindElement<HtmlElement>(By.Id("element-list"));
            hamlet = PageAlpha.FindElement<HtmlElement>(By.Id("hamlet"));

        }
        [Test]
        public void PreviousSibling()
        {
            Assert.AreEqual("ul", hamlet.PreviousSibling.TagName);
        }

        [Test]
        public void NextSibling()
        {
            Assert.AreEqual("iframe", hamlet.NextSibling.TagName);
        }

        [Test]
        public void TextContent()
        {
            Assert.That(hamlet.TextContent,
                Does.StartWith("\r\n")
                .And.Contains("...")
                .And.Contains("Something is rotten in the state of Denmark")
                .And.EndsWith(" "));
        }

        [Test]
        public void FirstChild()
        {
            Assert.AreEqual("p", hamlet.FirstChild.TagName);
        }

        [Test]
        public void LastChild()
        {
            Assert.AreEqual("br", hamlet.LastChild.TagName);
        }
    }
}

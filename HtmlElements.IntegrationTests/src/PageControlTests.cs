using HtmlElements.Elements;
using HtmlElements.LazyLoad;
using HtmlElements.Proxy;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.IntegrationTests
{
    public class PageControlTests : IntegrationTestFixture
    {
        [Test]
        public void ShouldSwitchToFrameWhenWorkingWithIt()
        {
            Assert.That(PageAlpha.BetaFrame, Is.Not.Null);

            PageAlpha.BetaFrame.SignIn("abra@gmail.com", "kadabra");
        }

        [Test]
        public void ControlShouldHaveMeaningfullStringRepresentation()
        {
            Assert.That(PageAlpha.BetaFrame.SubmitBtn.ToString(),
                ContainsSubstring(typeof(HtmlElement).Name)
                .And.ContainsSubstring(typeof(WebElementProxy).Name)
                .And.ContainsSubstring(typeof(WebElementLoader).Name)
                .And.ContainsSubstring(typeof(FrameWebElementProxy).Name));
        }

        [Test]
        public void ShouldThrowNoSuchElementException()
        {
            PageAlpha.Body.InnerHtml = "";

            Assert.Throws<NoSuchElementException>(() => PageAlpha.ElementListContainer.Click());
        }
    }
}
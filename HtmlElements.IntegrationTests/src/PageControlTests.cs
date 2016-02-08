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
                ContainsSubstring(typeof(HtmlElement).FullName)
                .And.ContainsSubstring(typeof(WebElementProxy).FullName)
                .And.ContainsSubstring(typeof(WebElementLoader).FullName)
                .And.ContainsSubstring(typeof(FrameWebElementProxy).FullName));
        }

        [Test]
        public void ShouldThrowNoSuchElementException()
        {
            PageAlpha.Body.InnerHtml = "";

            Assert.Throws<NoSuchElementException>(() => PageAlpha.ElementListContainer.Click());
        }
    }
}
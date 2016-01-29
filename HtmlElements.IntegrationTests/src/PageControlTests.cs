using HtmlElements.Elements;
using HtmlElements.IntegrationTests.Pages;
using HtmlElements.LazyLoad;
using HtmlElements.Proxy;
using NUnit.Framework;

namespace HtmlElements.IntegrationTests
{
    public class PageControlTests : IntegrationTestFixture
    {
        [Test]
        public void ShouldSwitchToFrameWhenWorkingWithIt()
        {
            var pageAltha = PageFactory.Create<PageAlpha>(WebDriver);

            Assert.That(pageAltha.BetaFrame, Is.Not.Null);

            pageAltha.BetaFrame.SignIn("abra@gmail.com", "kadabra");
        }

        [Test]
        public void ControlShouldHaveMeaningfullStringRepresentation()
        {
            var pageAltha = PageFactory.Create<PageAlpha>(WebDriver);

            Assert.That(pageAltha.BetaFrame.SubmitBtn.ToString(),
                ContainsSubstring(typeof(HtmlElement).FullName)
                .And.ContainsSubstring(typeof(WebElementProxy).FullName)
                .And.ContainsSubstring(typeof(WebElementLoader).FullName)
                .And.ContainsSubstring(typeof(FrameWebElementProxy).FullName));
        }
    }
}
﻿using HtmlElements.Elements;
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
            Assert.That(PageAlpha.NonCachedBetaFrame, Is.Not.Null);
            Assert.That(PageAlpha.CachedBetaFrame, Is.Not.Null);

            PageAlpha.NonCachedBetaFrame.SignIn("abra@gmail.com", "kadabra");
            PageAlpha.CachedBetaFrame.SignIn("abra@gmail.com", "kadabra");
        }

        [Test]
        public void ControlShouldHaveMeaningfullStringRepresentation()
        {
            Assert.That(PageAlpha.NonCachedBetaFrame.SubmitBtn.ToString(),
                Does.Contain(typeof(HtmlElement).Name)
                .And.Contain(typeof(WebElementProxy).Name)
                .And.Contain(typeof(WebElementLoader).Name)
                .And.Contain(typeof(FrameWebElementProxy).Name));
        }

        [Test]
        public void ShouldThrowNoSuchElementException()
        {
            PageAlpha.Body.InnerHtml = "";

            Assert.Throws<NoSuchElementException>(() => PageAlpha.ElementListContainer.Click());
        }
    }
}
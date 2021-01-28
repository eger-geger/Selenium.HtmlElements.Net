﻿using HtmlElements.Elements;
using HtmlElements.IntegrationTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.IntegrationTests.Elements
{
    public class HtmlPageTests : IntegrationTestFixture
    {
        [Test]
        public void ShouldLocateListOfCustomWebElements()
        {
            var elementList = PageAlpha.FindElements<HtmlElement>(By.CssSelector("li"));

            Assert.That(elementList, Is.Not.Null);
            Assert.That(elementList.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldLocateCustomWebElement()
        {
            var element = PageAlpha.FindElement<HtmlElement>(By.Id("hamlet"));

            Assert.That(element, Is.Not.Null);
            Assert.That(element.Text, Is.EqualTo("Something is rotten in the state of Denmark ..."));
        }

        [Test]
        public void ShouldProvideDocumentReadyState()
        {
            Assert.That(PageAlpha.ReadyState, Is.EqualTo(DocumentReadyState.Complete));
        }

        [Test]
        public void ShouldUseElementLocatorIfFindByAttributeIsNotProvided()
        {
            Assert.NotNull(PageAlpha.BetaFrame);
            Assert.NotNull(PageAlpha.BetaFrame.LoginField);
        }
    }
}
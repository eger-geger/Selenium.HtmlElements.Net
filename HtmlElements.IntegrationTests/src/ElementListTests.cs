using System;
using HtmlElements.Elements;
using HtmlElements.IntegrationTests.Pages;
using HtmlElements.LazyLoad;
using HtmlElements.Proxy;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.IntegrationTests
{
    public class ElementListTests : IntegrationTestFixture
    {
        [Test]
        public void ShouldReloadTransparentElementList()
        {
            var initElementCount = PageAlpha.ElementListItems.Count;

            PageAlpha.ElementListContainer.InnerHtml += "<li>new list item</li>";

            Assert.That(PageAlpha.ElementListItems.Count, Is.GreaterThan(initElementCount));
        }

        [Test]
        public void ShouldReloadTransparentListItemOnceDomElementGetUpdated()
        {
            var firstElement = PageAlpha.ElementListItems[0];

            PageAlpha.ElementListContainer.InnerHtml = "<li>alpha</li><li>beta</li>";

            Assert.That(firstElement.Text, Is.EqualTo("alpha"));
        }

        [Test]
        public void ShouldThrowNoSuchElementException()
        {
            var firstElement = PageAlpha.ElementListItems[0];

            PageAlpha.ElementListContainer.InnerHtml = String.Empty;

            Assert.That(() => firstElement.Text, Throws.InstanceOf<NoSuchElementException>());
        }

        [Test]
        public void CachedListShouldNotChangeWhenListGetChanged()
        {
            var initElementCount = PageAlpha.CachedElementListItems.Count;

            PageAlpha.ElementListContainer.InnerHtml += "<li>new list item</li>";

            Assert.That(PageAlpha.CachedElementListItems.Count, Is.EqualTo(initElementCount));
        }

        [Test]
        public void ShouldReloadCachedListOnceDomElementGetUpdated()
        {
            var firstElement = PageAlpha.CachedElementListItems[0];

            PageAlpha.ElementListContainer.InnerHtml = "<li>alpha</li><li>beta</li>";

            Assert.That(firstElement.Text, Is.EqualTo("alpha"));
        }

        [Test]
        public void ShouldProvideMeaningfullTextDescription()
        {
            var element = PageAlpha.CachedElementListItems[0];

            Assert.That(element.ToString(), 
                Does.Contain(typeof(HtmlElement).Name)
                .And.Contain(typeof(WebElementProxy).Name)
                .And.Contain(typeof(WebElementListItemLoader).Name)
                .And.Contain(typeof(WebElementListLoader).Name));
        }
    }
}
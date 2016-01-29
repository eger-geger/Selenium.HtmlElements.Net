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
        private PageAlpha _page;

        [SetUp]
        public void InitializePageObject()
        {
            _page = PageFactory.Create<PageAlpha>(WebDriver);
        }

        [Test]
        public void ShouldReloadTransparentElementList()
        {
            var initElementCount = _page.ElementListItems.Count;

            _page.ElementListContainer.InnerHtml += "<li>new list item</li>";

            Expect(_page.ElementListItems.Count, Is.GreaterThan(initElementCount));
        }

        [Test]
        public void ShouldReloadTransparentListItemOnceDomElementGetUpdated()
        {
            var firstElement = _page.ElementListItems[0];

            _page.ElementListContainer.InnerHtml = "<li>alpha</li><li>beta</li>";

            Expect(firstElement.Text, Is.EqualTo("alpha"));
        }

        [Test]
        public void ShouldThrowNoSuchElementException()
        {
            var firstElement = _page.ElementListItems[0];

            _page.ElementListContainer.InnerHtml = String.Empty;

            Expect(() => firstElement.Text, Throws.InstanceOf<NoSuchElementException>());
        }

        [Test]
        public void CachedListShouldNotChangeWhenListGetChanged()
        {
            var initElementCount = _page.CachedElementListItems.Count;

            _page.ElementListContainer.InnerHtml += "<li>new list item</li>";

            Expect(_page.CachedElementListItems.Count, Is.EqualTo(initElementCount));
        }

        [Test]
        public void ShouldReloadCachedListOnceDomElementGetUpdated()
        {
            var firstElement = _page.CachedElementListItems[0];

            _page.ElementListContainer.InnerHtml = "<li>alpha</li><li>beta</li>";

            Expect(firstElement.Text, Is.EqualTo("alpha"));
        }

        [Test]
        public void ShouldProvideMeaningfullTextDescription()
        {
            var element = _page.CachedElementListItems[0];

            Expect(element.ToString(), 
                ContainsSubstring(typeof(HtmlElement).FullName)
                .And.ContainsSubstring(typeof(WebElementProxy).FullName)
                .And.ContainsSubstring(typeof(WebElementListItemLoader).FullName)
                .And.ContainsSubstring(typeof(WebElementListLoader).FullName));
        }
    }
}
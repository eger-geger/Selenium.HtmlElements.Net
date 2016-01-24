using System;

using HtmlElements.Test.Pages;

using NUnit.Framework;

using OpenQA.Selenium;

namespace HtmlElements.Test.Integration {

    public class ElementListTests : IntegrationTestFixture {

        [Test]
        public void ShouldReloadElementList()
        {
            var page = PageFactory.Create<TestPageObject>(WebDriver);

            var initElementCount = page.ElementListItems.Count;

            page.ElementListContainer.InnerHtml += "<li>added item</li>";

            Expect(page.ElementListItems.Count, Is.GreaterThan(initElementCount));
        }

        [Test]
        public void ShouldThrowNoSuchElementException() {
            var page = PageFactory.Create<TestPageObject>(WebDriver);
            
            var listElement = page.ElementListItems[0];

            page.ElementListContainer.InnerHtml = String.Empty;

            Expect(() =>listElement.InnerHtml, Throws.InstanceOf<NoSuchElementException>());
        }

    }

}
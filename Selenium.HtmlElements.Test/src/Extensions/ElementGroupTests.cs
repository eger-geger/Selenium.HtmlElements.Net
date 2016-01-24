using HtmlElements.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Extensions
{
    public class ElementGroupTests : AssertionHelper {

        [Test]
        public void ShouldFindElmentsFromGroupInParentPage() {
            Expect(new ChildPageObject().GetElementsByGroup("some_group"), Is.Not.Empty);
        }

    }

    public class ParentPageObject {

        [ElementGroup("some_group")]
        private IWebElement _webElement;

    }

    public class ChildPageObject : ParentPageObject {

    }

}

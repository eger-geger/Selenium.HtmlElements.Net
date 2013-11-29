using HtmlElements.Demo.Pages;

using NUnit.Framework;

namespace HtmlElements.Demo.Tests {

    internal class DevLifeTests : BaseWebDriverTest {

        [Test]
        public void ShouldOpenFirstPage() {
            NavigateToUrl();

            var page = On<DevLifePage>().Pagination.OpenNextPage();
            page.Pagination.InnerHtml = "GAGA";

            Expect(CurrentUrl, Contains("1"));
        }

    }

}
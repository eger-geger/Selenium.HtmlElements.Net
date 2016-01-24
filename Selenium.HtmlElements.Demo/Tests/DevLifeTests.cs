using HtmlElements.Demo.Pages;

using NUnit.Framework;

namespace HtmlElements.Demo.Tests {

    internal class DevLifeTests : BaseWebDriverTest {

        [Test]
        public void ShouldOpenFirstPage() {
            NavigateToUrl();

            On<DevLifePage>().Pagination.OpenNextPage();

            Expect(CurrentUrl, Contains("1"));
        }

    }

}
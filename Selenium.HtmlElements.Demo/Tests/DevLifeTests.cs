using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Selenium.HtmlElements.Demo.Pages;

namespace Selenium.HtmlElements.Demo.Tests {

    class DevLifeTests : BaseWebDriverTest {

        [Test]
        public void ShouldOpenFirstPage() {
            NavigateToUrl();

            var page = On<DevLifePage>().Pagination.OpenNextPage();

            Expect(page.Pagination.CurrentNumber, Is.EqualTo(1));
            Expect(CurrentUrl, Contains("1"));
        }

    }
}

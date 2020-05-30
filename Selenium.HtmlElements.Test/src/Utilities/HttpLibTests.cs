using System.Collections.Generic;
using System.Collections.Specialized;
using HtmlElements.Utilities;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace HtmlElements.Test.Utilities
{
    public class HttpLibTests
    {
        private static IEnumerable<ITestCaseData> ParseQueryStringTests
        {
            get
            {
                yield return new TestCaseData(null).Returns(new NameValueCollection());
                yield return new TestCaseData(string.Empty).Returns(new NameValueCollection());
                
                yield return new TestCaseData("?name=Jack&age=14")
                    .Returns(new NameValueCollectionBuilder()
                        .Add("name", "Jack")
                        .Add("age", "14")
                        .NameValueCollection
                    );
                
                yield return new TestCaseData("?name=Jack&age=")
                    .Returns(new NameValueCollectionBuilder()
                        .Add("name", "Jack")
                        .Add("age", "")
                        .NameValueCollection
                    );
                
                yield return new TestCaseData("?name=Jack&age")
                    .Returns(new NameValueCollectionBuilder()
                        .Add("name", "Jack")
                        .Add("age", "")
                        .NameValueCollection
                    );
                
                yield return new TestCaseData("?name=Jack&name=Susan")
                    .Returns(new NameValueCollectionBuilder()
                        .Add("name", "Jack")
                        .Add("name", "Susan")
                        .NameValueCollection
                    );
                
                yield return new TestCaseData("?price=5%24&discount%20price=4.55%24")
                    .Returns(new NameValueCollectionBuilder()
                        .Add("price", "5$")
                        .Add("discount price", "4.55$")
                        .NameValueCollection
                    );
            }
        }

        [TestCaseSource(nameof(ParseQueryStringTests))]
        public NameValueCollection ShouldParseUrlQueryString(string queryString)
        {
            return HttpLib.ParseQueryString(queryString);
        }
    }
}
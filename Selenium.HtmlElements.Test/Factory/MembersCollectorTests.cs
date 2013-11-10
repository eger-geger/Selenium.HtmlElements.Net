using System.Collections.Generic;

using NUnit.Framework;

using OpenQA.Selenium;

using Selenium.HtmlElements.Elements;
using Selenium.HtmlElements.Factory;

namespace Selenium.HtmlElements.Test.Factory {

    [TestFixture]
    public class MembersCollectorTests : AssertionHelper {

        private class BaseClass {

            public HtmlElement PublicElementF;
            private IWebElement _privateElementF;
            private string _privateStringF;

            public IList<IWebElement> PublicListP { get; private set; }

            public HtmlElement PublicGetProp {
                get { return null; }
            }

        }

        private class ChildClass : BaseClass {

            protected IHtmlElement ProtectedElementF;
            public string PublicStringP { get; set; }

            protected IList<HtmlElement> ProtectedListP { get; private set; }

        }

        [Test]
        public void CollectMemersOfChildClass() {
            var memebers = MembersCollector.LocatableMembersFrom(typeof(ChildClass));

            Expect(memebers.Item1, Has.Count.EqualTo(3), "Fields was not collected");
            Expect(memebers.Item2, Has.Count.EqualTo(2), "Properties was not collected");
        }

    }

}
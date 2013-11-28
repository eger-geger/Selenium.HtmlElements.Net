using System.Collections.Generic;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using Selenium.HtmlElements.Elements;
using Selenium.HtmlElements.Internal;

namespace Selenium.HtmlElements.Test.Factory {

    [TestFixture]
    public class MembersCollectorTests : AssertionHelper {

        private class BaseClass {

            [FindsBy]
            public HtmlElement PublicElementF;

            [FindsBy]
            private IWebElement _privateElementF;
            
            [FindsBy]
            private string _privateStringF;

            [FindsBy]
            public IList<IWebElement> PublicListP { get; private set; }

            public HtmlElement PublicGetProp {
                get { return null; }
            }

        }

        private class ChildClass : BaseClass {

            protected IHtmlElement ProtectedElementF;

            [FindsBy]
            public string PublicStringP { get; set; }

            [FindsBy]
            protected IList<HtmlElement> ProtectedListP { get; private set; }

        }

        [Test]
        public void CollectMemersOfChildClass() {
            var memebers = MembersCollector.LocatableMembersFrom(typeof(ChildClass));

            Expect(memebers.Item1, Has.Count.EqualTo(2), "Fields was not collected");
            Expect(memebers.Item2, Has.Count.EqualTo(2), "Properties was not collected");
        }

    }

}
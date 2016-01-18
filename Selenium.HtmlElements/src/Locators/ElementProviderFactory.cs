using System;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace HtmlElements.Locators {

    internal class ElementProviderFactory {

        private readonly ISearchContext _searchContext;

        public ElementProviderFactory(ISearchContext searchContext) {
            _searchContext = searchContext;
        }

        public IElementProvider CreateLocator(MemberInfo memberInfo) {
            try {
                return new ElementProvider(_searchContext, ByFrom(memberInfo));
            } catch (Exception ex) {
                throw new ArgumentException(string.Format("Cannot build locator from [{0}]", memberInfo), ex);
            }
        }

        private static IEnumerable<By> ByFrom(MemberInfo memberInfo) {
            var attributes = memberInfo.GetCustomAttributes(typeof(FindsByAttribute), true);

            if (attributes.Length == 0) {
                yield return ByFactory.Create(How.Id, memberInfo.Name);
            } else {
                foreach (var attribute in attributes) {
                    yield return ByFactory.Create(attribute as FindsByAttribute);
                }
            }
        }

    }

}
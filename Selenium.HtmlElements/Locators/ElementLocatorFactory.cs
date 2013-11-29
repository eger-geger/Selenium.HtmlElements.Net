using System;
using System.Collections.Generic;
using System.Reflection;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Selenium.HtmlElements.Locators {

    internal class ElementLocatorFactory {

        private readonly ISearchContext _searchContext;

        public ElementLocatorFactory(ISearchContext searchContext) {
            _searchContext = searchContext;
        }

        public IElementLocator CreateLocator(MemberInfo memberInfo) {
            try {
                return new ElementLocator(_searchContext, ByFrom(memberInfo));
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
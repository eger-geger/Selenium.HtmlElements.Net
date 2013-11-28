using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Selenium.HtmlElements.Internal {

    internal class LocatorFactory {

        private readonly ISearchContext _searchContext;

        public LocatorFactory(ISearchContext searchContext) {
            _searchContext = searchContext;
        }

        public IElementLocator CreateLocator(MemberInfo memberInfo) {
            try {
                return new ElementLocator(_searchContext, ByFrom(memberInfo));
            } catch (Exception ex) {
                throw new InvalidOperationException(string.Format("Failed to build locator for {0}", memberInfo), ex);
            }
        }

        private static IEnumerable<By> ByFrom(MemberInfo memberInfo) {
            var findsByAttributes = memberInfo.GetCustomAttributes(typeof(FindsByAttribute), true)
                                              .Select(a => a as FindsByAttribute).ToList();

            if (findsByAttributes.Any()) return findsByAttributes.Select(ByFactory.From).ToList();

            return new List<By> {By.Name(memberInfo.Name), By.Id(memberInfo.Name), By.ClassName(memberInfo.Name)};
        }

    }

}
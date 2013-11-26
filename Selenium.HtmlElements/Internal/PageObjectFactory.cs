using System;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Internal {

    internal static class PageObjectFactory {

        public static object Create(Type type, ISearchContext context) {
            var emptyCtor = type.GetConstructor(new Type[0]);

            return emptyCtor != null
                ? emptyCtor.Invoke(new Object[0])
                : Activator.CreateInstance(type, context);
        }

    }

}
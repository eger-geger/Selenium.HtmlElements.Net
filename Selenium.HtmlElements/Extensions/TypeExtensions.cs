using System;
using System.Collections.Generic;

using OpenQA.Selenium;

namespace HtmlElements.Extensions {

    public static class TypeExtensions {

        public static bool IsWebElement(this Type type) {
            return typeof(IWebElement).IsAssignableFrom(type);
        }

        public static bool IsWebElementList(this Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>)
                   && IsWebElement(type.GetGenericArguments()[0]);
        }

    }

}
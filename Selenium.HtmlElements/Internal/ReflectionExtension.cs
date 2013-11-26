using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Internal {

    internal static class ReflectionExtension {

        public static bool IsWebElement(this Type type) {
            return typeof(IWebElement).IsAssignableFrom(type);
        }

        public static bool IsWebElementList(this Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>)
                && type.GetGenericArguments()[0].IsWebElement();
        }

        public static bool IsPropertyBackingField(this FieldInfo fieldInfo) {
            return Regex.IsMatch(fieldInfo.Name, "<.+>k__BackingField");
        }

    }

}
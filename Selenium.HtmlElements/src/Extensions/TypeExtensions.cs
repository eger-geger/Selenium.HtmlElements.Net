using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;

namespace HtmlElements.Extensions {

    internal static class TypeExtensions {

        internal static bool IsWebElement(this Type type) {
            return typeof(IWebElement).IsAssignableFrom(type);
        }

        internal static bool IsWebElementList(this Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>)
                && IsWebElement(type.GetGenericArguments()[0]);
        }

        internal static IEnumerable<MemberInfo> GetMembersFromGroups(this Type type, params String[] groups) {
            var membersList = new List<MemberInfo>();

            while (type != typeof(Object)) {
                membersList.AddRange(type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(
                        member => member.GetCustomAttributes(typeof(ElementGroupAttribute), true)
                            .Any(attribute => (attribute as ElementGroupAttribute).Groups.Any(groups.Contains))
                    )
                );

                type = type.BaseType;
            }

            return membersList.Distinct();
        }

    }

}
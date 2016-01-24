using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;

namespace HtmlElements.Extensions
{
    /// <summary>
    ///     Utility functions taking <see cref="Type" /> as it's first argument
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///     Determines weather instance of given type can be assigned to <see cref="IWebElement" /> reference.
        /// </summary>
        /// <param name="type">Type of interest</param>
        /// <returns>
        ///     <value>true</value>
        ///     if instance of this type is a we element and
        ///     <value>false</value>
        ///     otherwise
        /// </returns>
        public static bool IsWebElement(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            return typeof (IWebElement).IsAssignableFrom(type);
        }

        /// <summary>
        ///     Determines weather type describes list of web elements.
        /// </summary>
        /// <param name="type">Type of interest</param>
        /// <returns>
        ///     <value>true</value>
        ///     if type is <see cref="IList{T}" /> and generic argument is a web element, otherwise -
        ///     <value>false</value>
        /// </returns>
        public static bool IsWebElementList(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            var genericArguments = type.GetGenericArguments();

            return type.IsGenericType
                   && type.GetGenericTypeDefinition() == typeof (IList<>)
                   && genericArguments.Length == 1
                   && IsWebElement(genericArguments[0]);
        }

        internal static IEnumerable<MemberInfo> GetMembersFromGroups(this Type type, params String[] groups)
        {
            var membersList = new List<MemberInfo>();

            while (type != typeof (Object))
            {
                membersList.AddRange(type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic |
                                                     BindingFlags.Public)
                    .Where(
                        member => member.GetCustomAttributes(typeof (ElementGroupAttribute), true)
                            .Any(attribute => (attribute as ElementGroupAttribute).Groups.Any(groups.Contains))
                    )
                    );

                type = type.BaseType;
            }

            return membersList.Distinct();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
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

        /// <summary>
        ///     Retrieve metadata of all properties in current object hierarchy which can be assigned a web element or web element list.
        ///     Properties which cannot be read or written and also indexed properties are being excluded from search.
        /// </summary>
        /// <param name="type">Type being scanned</param>
        /// <param name="bindingFlags">A bitmask comprised that specify how the search is conducted</param>
        /// <returns>List of properties metadata which can be assigned a web element or list of web elements value</returns>
        public static IEnumerable<PropertyInfo> GetOwnAndInheritedProperties(this Type type, BindingFlags bindingFlags)
        {
            while (type != null && type != typeof(object))
            {
                foreach (var property in type.GetProperties(bindingFlags | BindingFlags.DeclaredOnly))
                {
                    if (!IsWebElementOrElementList(property.PropertyType))
                    {
                        continue;
                    }

                    if (!property.CanWrite || !property.CanRead)
                    {
                        continue;
                    }

                    if (property.GetIndexParameters().Length > 0)
                    {
                        continue;
                    }

                    yield return property;
                }

                type = type.BaseType;
            }
        }

        /// <summary>
        ///     Retrieve metadata of all fields in current object hierarchy which can be assigned a web element or web element list.
        /// </summary>
        /// <param name="type">Type being scanned</param>
        /// <param name="bindingFlags">A bitmask comprised that specify how the search is conducted</param>
        /// <returns>List of fields metadata which can be assigned a web element or list of web elements value</returns>
        public static IEnumerable<FieldInfo> GetOwnAndInheritedFields(this Type type, BindingFlags bindingFlags)
        {
            while (type != null && type != typeof(object))
            {
                foreach (var field in type.GetFields(bindingFlags | BindingFlags.DeclaredOnly))
                {
                    if (IsPropertyBackingField(field))
                    {
                        continue;
                    }

                    if (!IsWebElementOrElementList(field.FieldType))
                    {
                        continue;
                    }

                    yield return field;
                }

                type = type.BaseType;
            }
        }

        private static bool IsPropertyBackingField(FieldInfo fieldInfo)
        {
            return Regex.IsMatch(fieldInfo.Name, "<.+>k__BackingField");
        }

        private static bool IsWebElementOrElementList(Type type)
        {
            return type.IsWebElement() || type.IsWebElementList();
        }
    }
}
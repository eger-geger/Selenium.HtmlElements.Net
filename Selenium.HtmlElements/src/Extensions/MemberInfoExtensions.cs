using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HtmlElements.Extensions
{
    internal static class MemberInfoExtensions
    {
        private const BindingFlags DiscoverableBindingFlags = BindingFlags.DeclaredOnly
                                               | BindingFlags.Instance
                                               | BindingFlags.Public
                                               | BindingFlags.NonPublic;

        private static bool IsPropertyBackingField(this FieldInfo fieldInfo)
        {
            return Regex.IsMatch(fieldInfo.Name, "<.+>k__BackingField");
        }

        private static bool IsDiscoverable(this FieldInfo fieldInfo)
        {
            return (fieldInfo.FieldType.IsWebElement() || fieldInfo.FieldType.IsWebElementList())
                   && !fieldInfo.IsPropertyBackingField();
        }

        private static bool IsDiscoverable(this PropertyInfo propertyInfo)
        {
            return (propertyInfo.PropertyType.IsWebElement() || propertyInfo.PropertyType.IsWebElementList())
                   && propertyInfo.CanWrite && propertyInfo.GetIndexParameters().Length == 0;
        }

        public static IList<MemberInfo> DiscoverFieldAndProperties(this Type type)
        {
            if (type == null || type == typeof (object))
            {
                return new List<MemberInfo>();
            }

            var memberList = new List<MemberInfo>();

            memberList.AddRange(type.GetFields(DiscoverableBindingFlags).Where(IsDiscoverable));
            memberList.AddRange(type.GetProperties(DiscoverableBindingFlags).Where(IsDiscoverable));
            memberList.AddRange(type.BaseType.DiscoverFieldAndProperties());

            return memberList;
        }

        public static IEnumerable<PropertyInfo> GetOwnAndInheritedProperties(this Type type, BindingFlags bindingFlags)
        {
            while (type != null && type != typeof(object))
            {
                foreach (var property in type.GetProperties(bindingFlags))
                {
                    yield return property;
                }

                type = type.BaseType;
            }
        }

        public static IEnumerable<FieldInfo> GetOwnAndInheritedFields(this Type type, BindingFlags bindingFlags)
        {
            while (type != null && type != typeof(object))
            {
                foreach (var field in type.GetFields(bindingFlags))
                {
                    yield return field;
                }

                type = type.BaseType;
            }
        }

        public static Type GetPropertyOrFieldType(this MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo) memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo) memberInfo).PropertyType;
            }

            throw new ArgumentException($"Unsupported class member: {memberInfo.Name}", nameof(memberInfo));
        }

        public static Object ReadPropertyOrFieldValue(this MemberInfo memberInfo, Object target)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo) memberInfo).GetValue(target);
                case MemberTypes.Property:
                    return ((PropertyInfo) memberInfo).GetValue(target, null);
            }

            throw new InvalidOperationException($"Unable to read {target.GetType()}.{memberInfo.Name}");
        }
    }
}
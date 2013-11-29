using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HtmlElements.Extensions {

    internal static class MemberInfoExtensions {

        private const BindingFlags BindFlags = BindingFlags.DeclaredOnly
                                                  | BindingFlags.Instance
                                                  | BindingFlags.Public
                                                  | BindingFlags.NonPublic;

        public static bool IsPropertyBackingField(this FieldInfo fieldInfo) {
            return Regex.IsMatch(fieldInfo.Name, "<.+>k__BackingField");
        }

        private static bool IsLocatable(this FieldInfo self) {
            return (self.FieldType.IsWebElement() || self.FieldType.IsWebElementList()) 
                && !self.IsPropertyBackingField();
        }

        private static bool IsLocatable(this PropertyInfo self) {
            return (self.PropertyType.IsWebElement() || self.PropertyType.IsWebElementList()) 
                && self.CanWrite && self.GetIndexParameters().Length == 0;
        }

        public static IList<MemberInfo> LocatableMembers(this Type type) {
            if (type == null || type == typeof(object)) return new List<MemberInfo>();

            var memberList = new List<MemberInfo>();

            memberList.AddRange(type.GetFields(BindFlags).Where(IsLocatable));
            memberList.AddRange(type.GetProperties(BindFlags).Where(IsLocatable));
            memberList.AddRange(type.BaseType.LocatableMembers());

            return memberList;
        }

    }

}
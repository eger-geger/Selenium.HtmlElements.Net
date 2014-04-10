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

        public static Type GetPropertyOrFieldType(this MemberInfo memberInfo) {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo) memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo) memberInfo).PropertyType;
            }

            throw new ArgumentException("neither FieldInfo neither PropertyInfo", "memberInfo");
        }

        public static Object GetPropertyOrFieldValue(this MemberInfo memberInfo, Object target) {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(target);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(target, null);
            }

            throw new InvalidOperationException(String.Format("cannot extract value [{0}] from [{1}]", memberInfo, target));
        }

    }

}
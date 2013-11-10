using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Selenium.HtmlElements.Factory {

    internal static class MembersCollector {

        private const BindingFlags BindingOptions =
            BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static Tuple<IEnumerable<FieldInfo>, IEnumerable<PropertyInfo>> LocatableMembersFrom(Type type) {
            var properties = new List<PropertyInfo>();
            var fields = new List<FieldInfo>();

            if (type != null && type != typeof(object)) {
                fields.AddRange(type.GetFields(BindingOptions)
                    .Where(f => !f.IsPropertyBackingField() && IsLocatable(f.FieldType)));

                properties.AddRange(type.GetProperties(BindingOptions)
                    .Where(p => p.CanWrite && IsLocatable(p.PropertyType)));

                var baseTypeLocatableMembers = LocatableMembersFrom(type.BaseType);

                fields.AddRange(baseTypeLocatableMembers.Item1);
                properties.AddRange(baseTypeLocatableMembers.Item2);
            }

            return new Tuple<IEnumerable<FieldInfo>, IEnumerable<PropertyInfo>>(fields, properties);
        }

        private static bool IsLocatable(Type type) {
            return type.IsWebElement() || type.IsWebElementList();
        }

    }

}
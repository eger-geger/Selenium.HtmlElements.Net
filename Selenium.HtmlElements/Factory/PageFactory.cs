using System;
using System.Linq;
using System.Reflection;

using OpenQA.Selenium;

using Selenium.HtmlElements.Locators;

namespace Selenium.HtmlElements.Factory {

    public static class PageFactory {

        public static T InitElementsIn<T>(ISearchContext context) where T : class {
            return InitElementsIn(typeof(T), context) as T;
        }

        public static object InitElementsIn(Type type, ISearchContext context) {
            try {
                var target = CreateTargetInstance(type, context);

                InitElementsIn(target, context);

                return target;
            }
            catch (SystemException ex) {
                throw new InvalidOperationException(string.Format("Failed to create instance of {0}", type), ex);
            }
        }

        private static object CreateTargetInstance(Type type, ISearchContext context) {
            try {
                return Activator.CreateInstance(type, context);
            }
            catch (MissingMethodException) {
                return Activator.CreateInstance(type);
            }
        }

        public static void InitElementsIn(object target, ISearchContext context) {
            if (target == null) throw new ArgumentNullException("target", "not initialized");

            var locatableMembers = MembersCollector.LocatableMembersFrom(target.GetType());

            var locatorFactory = new LocatorFactory(context);

            foreach (var field in locatableMembers.Item1.Where(f => f.GetValue(target) == null)) {
                field.SetValue(target, ValueFor(field, locatorFactory));
            }

            foreach (var property in locatableMembers.Item2.Where(p => p.GetValue(target, null) == null)) {
                property.SetValue(target, ValueFor(property, locatorFactory), null);
            }
        }

        private static object ValueFor(MemberInfo memberInfo, LocatorFactory locatorFactory) {
            Type memberType;

            if (memberInfo is FieldInfo) memberType = (memberInfo as FieldInfo).FieldType;
            else if (memberInfo is PropertyInfo) memberType = (memberInfo as PropertyInfo).PropertyType;
            else throw new ArgumentException("Should be property or field", "memberInfo");

            return ElementFactory.Create(memberType, locatorFactory.CreateLocator(memberInfo));
        }

    }

}
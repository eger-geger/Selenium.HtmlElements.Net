using System;
using System.Linq;
using System.Reflection;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using Selenium.HtmlElements.Internal;

namespace Selenium.HtmlElements {

    public static class PageObjectActivator {

        public static object Activate(Type type, ISearchContext context) {
            var instance = PageObjectFactory.Create(type, context);
            Activate(instance, context);
            return instance;
        }

        public static T Activate<T>(ISearchContext context) where T : class {
            return Activate(typeof(T), context) as T;
        }

        public static void Activate(object target, ISearchContext context) {
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
            return ElementFactory.Create(TypeOf(memberInfo), locatorFactory.CreateLocator(memberInfo),
                memberInfo.IsDefined(typeof(CacheLookupAttribute), true));
        }

        private static Type TypeOf(MemberInfo memberInfo) {
            if (memberInfo is FieldInfo) return (memberInfo as FieldInfo).FieldType;
            if (memberInfo is PropertyInfo) return (memberInfo as PropertyInfo).PropertyType;
            throw new ArgumentException("Should be property or field", "memberInfo");
        }

    }

}
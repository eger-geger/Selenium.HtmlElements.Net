using System;
using System.Linq;
using System.Reflection;

using HtmlElements.Locators;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using HtmlElements.Extensions;

namespace HtmlElements {

    public static class ElementActivator {

        public static object Activate(Type type, ISearchContext context) {
            if (type == null) throw new ArgumentNullException("type");
            if (type == null) throw new ArgumentNullException("context");

            var instance = ObjectFactory.CreatePageObject(type, context);

            Activate(instance, context);

            return instance;
        }

        public static T Activate<T>(ISearchContext context) where T : class {
            return Activate(typeof(T), context) as T;
        }

        public static void Activate(object target, ISearchContext context) {
            if (target == null) throw new ArgumentNullException("target");
            if (context == null) throw new ArgumentNullException("context");

            var locatorFactory = new ElementLocatorFactory(context);

            var members = target.GetType().LocatableMembers();

            foreach (var field in members.OfType<FieldInfo>().Where(f => f.GetValue(target) == null)) {
                field.SetValue(target, ValueFor(field, locatorFactory));
            }

            foreach (var property in members.OfType<PropertyInfo>().Where(p => p.GetValue(target, null) == null)) {
                property.SetValue(target, ValueFor(property, locatorFactory), null);
            }
        }

        private static object ValueFor(MemberInfo memberInfo, ElementLocatorFactory locatorFactory) {
            return ElementFactory.Create(TypeOf(memberInfo), locatorFactory.CreateLocator(memberInfo),
                memberInfo.IsDefined(typeof(CacheLookupAttribute), true));
        }

        private static Type TypeOf(MemberInfo memberInfo) {
            if (memberInfo is FieldInfo) return (memberInfo as FieldInfo).FieldType;
            if (memberInfo is PropertyInfo) return (memberInfo as PropertyInfo).PropertyType;
            throw new ArgumentException("Unexpected type member", "memberInfo");
        }

    }

}
using System;
using System.Linq;
using System.Reflection;
using HtmlElements.Elements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace HtmlElements.Locators
{
    internal class BySelf
    {
        private const string LocatorMemberName = "Locator";

        public static By Locate(MemberInfo memberInfo, Type elementType)
        {
            var locator = elementType.GetMember(LocatorMemberName).FirstOrDefault();
            if (!typeof(HtmlElement).IsAssignableFrom(elementType) || locator == null)
                throw new ArgumentException(
                    $"{memberInfo.Name} of {memberInfo.DeclaringType?.Name} should either use {nameof(FindsByAttribute)} or be derived from {nameof(HtmlElement)} class and implement public member '{nameof(By)} {LocatorMemberName}'");

            var instance = Activator.CreateInstance(elementType, default(HtmlElement));
            var value = (locator as FieldInfo)?.GetValue(instance) ?? (locator as PropertyInfo)?.GetValue(instance);
            if (value as By == null)
                throw new ArgumentException($"{elementType.Name}.{LocatorMemberName} is not set");

            return value as By;
        }
    }
}
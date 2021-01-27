using System;
using System.Linq;
using System.Reflection;
using HtmlElements.Elements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace HtmlElements.Locators
{
    internal class ByDefaultLocator
    {
        private const string LocatorMemberName = "Locator";

        public static By Locate(MemberInfo memberInfo, Type elementType)
        {
            var errorMessage =
                $"{memberInfo.Name} of {memberInfo.DeclaringType?.Name} should either use {nameof(FindsByAttribute)} or be derived from {nameof(HtmlElement)} class and implement public member '{nameof(By)} {LocatorMemberName}'";

            if (!typeof(HtmlElement).IsAssignableFrom(elementType))
                throw new ArgumentException(errorMessage);

            return (Activator.CreateInstance(elementType, default(HtmlElement)) as HtmlElement)?.DefaultLocator ?? By.Id(memberInfo.Name);
        }
    }
}
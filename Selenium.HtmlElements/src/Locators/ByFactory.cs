using System;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace HtmlElements.Locators {

    internal static class ByFactory {

        public static By Create(How how, string usingValue, Type customFinderType = null) {
            if(string.IsNullOrWhiteSpace(usingValue) && how == How.Custom)
                throw new ArgumentException(string.Format("[Using] should not be empty when [How={0}]", how));

            switch (how) {
                case How.Id:
                    return By.Id(usingValue);
                case How.Name:
                    return By.Name(usingValue);
                case How.TagName:
                    return By.TagName(usingValue);
                case How.ClassName:
                    return By.ClassName(usingValue);
                case How.CssSelector:
                    return By.CssSelector(usingValue);
                case How.LinkText:
                    return By.LinkText(usingValue);
                case How.PartialLinkText:
                    return By.PartialLinkText(usingValue);
                case How.XPath:
                    return By.XPath(usingValue);
                case How.Custom:
                    return Create(customFinderType, usingValue);
            }

            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                "Cannot construct [By] from [How={0}], [Using={1}]", how, usingValue));
        }

        public static By Create(Type customFinderType, string usingValue) {
            if (customFinderType == null) {
                throw new ArgumentException("Cannot use [How.Custom] without supplying a [CustomFinderType]");
            }

            if (!customFinderType.IsSubclassOf(typeof(By))) {
                throw new ArgumentException("[CustomFinderType] must be a descendant of [By] class");
            }

            return Activator.CreateInstance(customFinderType, usingValue) as By;
        }

        public static By Create(FindsByAttribute attribute) {
            return Create(attribute.How, attribute.Using, attribute.CustomFinderType);
        }

    }

}
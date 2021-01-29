using System;
using System.Globalization;
using HtmlElements.Elements;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace HtmlElements.Locators
{
    internal static class ByFactory
    {
        public static By Create(How how, string usingValue, Type customFinderType = null)
        {
            return how switch
            {
                How.Id => By.Id(usingValue),
                How.Name => By.Name(usingValue),
                How.TagName => By.TagName(usingValue),
                How.ClassName => By.ClassName(usingValue),
                How.CssSelector => By.CssSelector(usingValue),
                How.LinkText => By.LinkText(usingValue),
                How.PartialLinkText => By.PartialLinkText(usingValue),
                How.XPath => By.XPath(usingValue),
                How.Custom => Create(customFinderType, usingValue),
                _ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    "Cannot construct [By] from [How={0}], [Using={1}]", how, usingValue))
            };
        }

        public static By Create(Type customFinderType, string usingValue)
        {
            if (customFinderType == null)
            {
                throw new ArgumentException("Cannot use [How.Custom] without supplying a [CustomFinderType]");
            }

            if (!customFinderType.IsSubclassOf(typeof(By)))
            {
                throw new ArgumentException("[CustomFinderType] must be a descendant of [By] class");
            }

            return Activator.CreateInstance(customFinderType, usingValue) as By;
        }

        public static By Create(FindsByAttribute attribute)
        {
            return Create(attribute.How, attribute.Using, attribute.CustomFinderType);
        }

        public static By Create(ElementLocatorAttribute attribute)
        {
            return Create(attribute.How, attribute.Using, attribute.CustomFinderType);
        }
    }
}
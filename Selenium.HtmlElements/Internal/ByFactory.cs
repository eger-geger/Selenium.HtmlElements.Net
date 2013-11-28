using System;
using System.Globalization;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Selenium.HtmlElements.Internal {

    /// <summary cref="OpenQA.Selenium.Support.PageObjects.ByFactory">
    ///     Copied from WebDriver sources
    /// </summary>
    internal static class ByFactory {

        /// <summary>
        ///     Gets an instance of the <see cref="By" /> class based on the specified attribute.
        /// </summary>
        /// <param name="attribute">
        ///     The <see cref="FindsByAttribute" /> describing how to find the element.
        /// </param>
        /// <returns>
        ///     An instance of the <see cref="By" /> class.
        /// </returns>
        public static By From(FindsByAttribute attribute) {
            var how = attribute.How;
            var usingValue = attribute.Using;

            if (string.IsNullOrEmpty(usingValue)) {
                throw new InvalidOperationException(string.Format("Wrong locator by [{0}] using [{1}]", how, usingValue));
            }

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
                    if (attribute.CustomFinderType == null) throw new ArgumentException("Cannot use How.Custom without supplying a custom finder type");

                    if (!attribute.CustomFinderType.IsSubclassOf(typeof(By))) throw new ArgumentException("Custom finder type must be a descendent of the By class");

                    var constructor = attribute.CustomFinderType.GetConstructor(new[] {typeof(string)});

                    if (constructor == null) {
                        throw new ArgumentException(
                            "Custom finder type must expose a public constructor with a string argument");
                    }

                    return constructor.Invoke(new object[] {usingValue}) as By;
            }

            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                "Did not know how to construct How from how {0}, using {1}", how,
                usingValue));
        }

    }

}
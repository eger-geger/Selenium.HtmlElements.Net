using System;
using SeleniumExtras.PageObjects;

namespace HtmlElements.Elements
{
    /// <summary>
    /// Defines default <see cref="HtmlElement"/> locator which is used whenever specific element type
    /// added to page object without <see cref="FindsByAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ElementLocatorAttribute : Attribute
    {
        /// <inheritdoc cref="FindsByAttribute.How"/>
        public How How { get; set; }

        /// <inheritdoc cref="FindsByAttribute.Using"/>
        public string Using { get; set; }

        /// <inheritdoc cref="FindsByAttribute.CustomFinderType"/>
        public Type CustomFinderType { get; set; }
        
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements
{
    /// <summary>
    ///     Named group of related <see cref="IWebElement">WebElements</see>.
    /// </summary>
    public class ElementGroup
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.DeclaredOnly
                                                  | System.Reflection.BindingFlags.Instance
                                                  | System.Reflection.BindingFlags.Public
                                                  | System.Reflection.BindingFlags.NonPublic;

        private readonly string[] _groups;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ElementGroup"/> class.
        /// </summary>
        /// <param name="groups">
        ///     List of group names WebElements belongs to.
        /// </param>
        public ElementGroup(params string[] groups)
        {
            _groups = groups;
        }

        /// <summary>
        ///     Retrieves all WebElements marked with <see cref="ElementGroupAttribute"/> and matching group name.
        /// </summary>
        /// <param name="pageObject">
        ///     Page object which fields and properties is being scanned.
        /// </param>
        /// <returns>
        ///     List of WebElements which belong to current group.
        /// </returns>
        public IDictionary<string, IWebElement> GetElements(object pageObject)
        {
            var pageObjectType = pageObject.GetType();

            IDictionary<string, IWebElement> elements = new Dictionary<string, IWebElement>();

            foreach (var field in pageObjectType.GetOwnAndInheritedFields(BindingFlags))
            {
                if (!field.FieldType.IsWebElement())
                {
                    continue;
                }

                IList<ElementGroupAttribute> attributes = field
                    .GetCustomAttributes(typeof(ElementGroupAttribute), true)
                    .OfType<ElementGroupAttribute>()
                    .ToList();

                if (attributes.Count == 0)
                {
                    continue;
                }

                if (attributes.Any(attribute => attribute.Groups.Any(group => _groups.Contains(group))))
                {
                    elements.Add(field.Name, field.GetValue(pageObject) as IWebElement);
                }
            }

            foreach (var property in pageObjectType.GetOwnAndInheritedProperties(BindingFlags))
            {
                if (!property.PropertyType.IsWebElement())
                {
                    continue;
                }

                IList<ElementGroupAttribute> attributes = property
                    .GetCustomAttributes(typeof(ElementGroupAttribute), true)
                    .OfType<ElementGroupAttribute>()
                    .ToList();

                if (attributes.Count == 0)
                {
                    continue;
                }

                if (attributes.Any(attribute => attribute.Groups.Any(group => _groups.Contains(group))))
                {
                    elements.Add(property.Name, property.GetValue(pageObject, null) as IWebElement);
                }
            }

            return elements;
        }
    }
}
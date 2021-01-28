using System;

namespace HtmlElements
{
    /// <summary>
    ///     Allows to mark a page object field or property as part of named group. 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ElementGroupAttribute : Attribute
    {
        private readonly string[] _groups;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ElementGroupAttribute"/> class with provided group names.
        /// </summary>
        /// <param name="groups">
        ///     List of group names to which element belongs.
        /// </param>
        public ElementGroupAttribute(params string[] groups)
        {
            _groups = groups;
        }

        /// <summary>
        ///     List of group names.
        /// </summary>
        public string[] Groups => _groups;
    }
}
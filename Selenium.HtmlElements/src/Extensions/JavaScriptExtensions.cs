using System;
using HtmlElements.Elements;

namespace HtmlElements.Extensions {

    /// <summary>
    ///     Provides methods of manipulating DOM element via JavaScript
    /// </summary>
    public static class JavaScriptExtensions {

        /// <summary>
        ///     Execute JavaScript code in browser replacing all occurrences of <c>{self}</c> with actual DOM element being pointed to by current web element.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="jsSnippet">JavaScript snippet to execute in browser</param>
        /// <param name="arguments">Arguments passed to JavaScript snippet</param>
        /// <returns>Result of the script execution</returns>
        public static object ExecuteScriptOnSelf(this HtmlElement element, string jsSnippet, params object[] arguments)
        {
            var extendedArguments = new object[arguments.Length + 1];
            extendedArguments[arguments.Length] = element.WrappedElement;
            arguments.CopyTo(extendedArguments, 0);

            return element.ExecuteScript(
                jsSnippet.Replace("{self}", string.Format("arguments[{0}]", arguments.Length)), extendedArguments
            );
        }

        /// <summary>
        ///     Execute JavaScript code in browser replacing all occurrences of <c>{self}</c> with actual 
        ///     DOM element being pointed to by current web element and convert result to specific type.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="jsSnippet">JavaScript snippet to execute in browser</param>
        /// <param name="arguments">Arguments passed to JavaScript snippet</param>
        /// <returns>Result of the script execution or default value for a given type</returns>
        public static TReturn ExecuteScriptOnSelf<TReturn>(this HtmlElement element, string jsSnippet, params object[] arguments)
        {
            var result = element.ExecuteScriptOnSelf(jsSnippet, arguments);

            if (result is TReturn)
            {
                return (TReturn) result;
            }

            return default(TReturn);
        }

        /// <summary>
        ///     Checks attribute exists in DOM element
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>
        ///     <c>true</c> if attribute exists and <c>false</c> otherwise
        /// </returns>
        public static bool HasAttribute(this HtmlElement element, string attributeName) {
            return element.ExecuteScriptOnSelf<bool?>(
                "return {self}.hasAttribute(arguments[0]);", attributeName
            ).GetValueOrDefault(false);
        }

        /// <summary>
        ///     Set DOM element attribute value
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="attributeValue">Attribute value</param>
        public static void SetAttribute(this HtmlElement element, string attributeName, string attributeValue) {
            element.ExecuteScriptOnSelf("{self}.setAttribute(arguments[0], arguments[1]);", attributeName, attributeValue);
        }

        /// <summary>
        ///     Remove DOM element attribute
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="attributeName">Attribute name</param>
        public static void RemoveAttribute(this HtmlElement element, string attributeName) {
            element.ExecuteScriptOnSelf("{self}.removeAttribute(arguments[0]);", attributeName);
        }

        /// <summary>
        ///     Checks weather property exist in given DOM element object
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>
        ///     <c>true</c> if property with a given name exist and <c>false</c> otherwise
        /// </returns>
        public static bool HasProperty(this HtmlElement element, string propertyName)
        {
            return element.ExecuteScriptOnSelf<bool>("return !!{self}[arguments[0]];");
        }

        /// <summary>
        ///     Set property value for given DOM element object
        /// </summary>
        /// <param name="element"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public static void SetPropery(this HtmlElement element, string propertyName, object propertyValue) {
            element.ExecuteScriptOnSelf("{self}[arguments[0]] = arguments[1];", propertyName, propertyValue);
        }

        /// <summary>
        ///     Get value of a given DOM element property
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Value of the property</returns>
        public static T GetProperty<T>(this HtmlElement element, string propertyName)
        {
            return (T)element.ExecuteScriptOnSelf("return {self}[arguments[0]];", propertyName);
        }

    }

}
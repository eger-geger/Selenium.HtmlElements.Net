using System;

namespace Selenium.HtmlElements.Elements {

    public static class JsExtension {

        private const string JsUndefinedString = "undefined";

        public static bool HasAttribute(this HtmlElement self, string name) {
            var script = string.Format("return {{self}}.hasAttribute('{0}');", name);

            var retval = self.ExecuteScriptOnSelf(script) as Boolean?;

            return retval != null && retval.Value;
        }

        public static void SetAttribute(this HtmlElement self, string name, string value) {
            self.ExecuteScriptOnSelf(string.Format("{{self}}.setAttribute('{0}', '{1}');", name, value));
        }

        public static void RemoveAttribute(this HtmlElement self, string name) {
            self.ExecuteScriptOnSelf(string.Format("{{self}}.removeAttribute('{0}')", name));
        }

        public static bool HasProperty(this HtmlElement self, string name) {
            var property = self.ExecuteScriptOnSelf(string.Format("return {{self}}.{0};", name));

            return property != null && !property.Equals(JsUndefinedString);
        }

        public static void SetDomElementPropery(this HtmlElement self, string name, object value) {
            self.ExecuteScriptOnSelf(string.Format("{{self}}.{0} = arguments[0];", name), value);
        }

        public static object GetDomElementProperty(this HtmlElement self, string name) {
            return self.ExecuteScriptOnSelf(string.Format("return {{self}}.{0};", name));
        }

    }

}
using System;

namespace HtmlElements.Locators {

    /// <summary>
    ///     Wraps locator clause with jQuery('${locator}').get() 
    /// </summary>
    public class ByJquery : ByJavaScript {

        public ByJquery(String locator) : base(ToJavaScriptLocator(locator)) {
            Description = string.Format("By.jQuery: {0}", locator);
        }

        private static string ToJavaScriptLocator(String jsLocator) {
            return string.Format(@"jQuery(""{0}"").get()", jsLocator);
        }

    }

}
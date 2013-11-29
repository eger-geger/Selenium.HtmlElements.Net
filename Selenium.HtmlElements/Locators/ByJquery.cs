namespace Selenium.HtmlElements.Locators {

    public class ByJquery : ByJavaScript {

        public ByJquery(string locator) : base(ToJavaScriptLocator(locator)) {
            Description = string.Format("By.jQuery: {0}", locator);
        }

        private static string ToJavaScriptLocator(string jsLocator) {
            return string.Format(@"jQuery(""{0}"").get()", jsLocator);
        }

    }

}
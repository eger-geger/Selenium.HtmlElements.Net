namespace Selenium.HtmlElements {

    public class ByJquery : ByJs {

        public ByJquery(string locator) : base(WrapWithJquery(locator)) {
            Description = string.Format("By.JQuery: {0}", locator);
        }

        private static string WrapWithJquery(string jsLocator) {
            return string.Format("jQuery(\"{0}\").get()", jsLocator);
        }

    }

}
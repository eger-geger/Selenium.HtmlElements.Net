namespace HtmlElements.Locators
{
    /// <summary>
    ///     Use jQuery to locate element or list of elements
    /// </summary>
    public class ByJquery : ByJavaScript
    {
        /// <summary>
        ///     Creates new instance of jQuery element finder.
        /// </summary>
        /// <param name="jqSelector">
        ///     jQuery compatible element selector.
        /// </param>
        public ByJquery(string jqSelector) : base("window.jQuery && jQuery(arguments[0]).get() || []", jqSelector)
        {
            Description = string.Format("By.jQuery: {0}", jqSelector);
        }
    }
}
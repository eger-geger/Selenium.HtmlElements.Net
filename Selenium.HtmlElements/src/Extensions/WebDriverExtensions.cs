using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Extensions {

    /// <summary>
    ///     Set of additional methods of <see cref="IWebDriver"/>
    /// </summary>
    public static class WebDriverExtensions {

        /// <summary>
        ///     Locates last window handler and make it active.
        /// </summary>
        /// <param name="webDriver">WebDriver instance</param>
        [Obsolete("This method is deprecated, please use native webdriver switch to window functionality.")]
        public static void SwitchToLastOpenedWindow(this IWebDriver webDriver) {
            webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
        }

        /// <summary>
        ///     Perform action and wait for 10 seconds until new browser tab will be opened.
        /// </summary>
        /// <param name="webDriver">WebDriver instance</param>
        /// <param name="command">Action which should trigger new browser tab</param>
        /// <param name="message">Error message used when command expires</param>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown if new tab did not open after 10 seconds
        /// </exception>
        /// <returns>List of new window handles.</returns>
        public static IList<string> WaitUntilNewWindowOpened(this IWebDriver webDriver, Action command, String message = null) {
            var initialWindowHandles = webDriver.WindowHandles;

            command();

            webDriver.WaitUntil(s => s.WindowHandles.Count > initialWindowHandles.Count, message ?? "New browser window did not open after 10 seconds");

            return webDriver.WindowHandles.Where(windowHandle => !initialWindowHandles.Contains(windowHandle)).ToList();
        }

        /// <summary>
        ///     Perform action and wait until new browser tab will be opened.
        /// </summary>
        /// <param name="webDriver">WebDriver instance</param>
        /// <param name="command">Action which should trigger new browser tab</param>
        /// <param name="commandTimeout">Time after which command expires</param>
        /// <param name="message">Error message used when command expires</param>
        /// <exception cref="WebDriverTimeoutException">
        ///     Thrown if new tab did not open after 10 seconds
        /// </exception>
        public static void WaitUntilNewWindowOpened(this IWebDriver webDriver, Action command, TimeSpan commandTimeout, String message = null) {
            var initWindowCount = webDriver.WindowHandles.Count;

            command();

            webDriver.WaitUntil(s => s.WindowHandles.Count > initWindowCount, commandTimeout, 
                message ?? String.Format("New browser window did not open after {0}", commandTimeout)
            );
        }

        /// <summary>
        ///     Determine weather all of the browser windows were closed.
        /// </summary>
        /// <param name="webDriver">Target web driver</param>
        /// <returns><value>true</value> if there are no active windows and <value>false</value> if there is at least one</returns>
        public static bool IsClosed(this IWebDriver webDriver) {
            try {
                return webDriver.WindowHandles.Count == 0;
            } catch (Exception) {
                return true;
            }
        }

        /// <summary>
        ///     Write page to file.
        /// </summary>
        /// <param name="webDriver">Target web driver</param>
        /// <param name="path">File path</param>
        public static void SavePageSource(this IWebDriver webDriver, String path) {
            File.WriteAllText(path, webDriver.PageSource);
        }

        /// <summary>
        ///     Take screen-shot and save it to file.
        /// </summary>
        /// <param name="webDriver">WebDriver capable of taking screen-shots</param>
        /// <param name="imagePath">Screen-shot file path</param>
        /// <param name="imageFormat">Image format in which screen-shot will be saved</param>
        public static void SavePageImage(this ITakesScreenshot webDriver, string imagePath, ScreenshotImageFormat imageFormat) {
            webDriver.GetScreenshot().SaveAsFile(imagePath, imageFormat);
        }

        /// <summary>
        ///     Scrolls the document to the specified coordinates.
        /// </summary>
        /// <param name="webDriver">WebDriver capable of executing JavaScript</param>
        /// <param name="xpos">The coordinate to scroll to, along the x-axis (horizontal), in pixels</param>
        /// <param name="ypos">The coordinate to scroll to, along the y-axis (vertical), in pixels</param>
        /// <exception cref="ArgumentException">
        ///     Throw if WebDriver cannot execute JavaScript
        /// </exception>
        public static void ScrollTo(this IWebDriver webDriver, long xpos, long ypos)
        {
            var jsExecutor = webDriver.ToJavaScriptExecutor();

            if (jsExecutor == null)
            {
                throw new ArgumentException("WebDriver cannot execute JavaScript", "webDriver");
            }

            jsExecutor.ExecuteScript(String.Format("window.scrollTo({0}, {1});", xpos, ypos));
        }

        /// <summary>
        ///      Scrolls the document by the specified number of pixels.
        /// </summary>
        /// <param name="webDriver">WebDriver capable of executing JavaScript</param>
        /// <param name="offsetX">
        ///     How many pixels to scroll by, along the x-axis (horizontal). 
        ///     Positive values will scroll to the left, while negative values will scroll to the right.
        /// </param>
        /// <param name="offsetY">
        ///     How many pixels to scroll by, along the y-axis (vertical). 
        ///     Positive values will scroll down, while negative values scroll up.
        /// </param>
        public static void ScrollBy(this IWebDriver webDriver, long offsetX, long offsetY) {
            webDriver.ToJavaScriptExecutor().ExecuteScript(String.Format("window.scrollBy({0}, {1});", offsetX, offsetY));
        }

        /// <summary>
        ///     Execute command, wait 10 seconds until new browser tab is opened and switch to it.
        /// </summary>
        /// <param name="webDriver">Target WebDriver</param>
        /// <param name="command">Command which should trigger new browser tab</param>
        /// <param name="message">Error message used when command expires</param>
        public static void OpenNewWindow(this IWebDriver webDriver, Action command, String message = null) {
            webDriver.SwitchTo().Window(webDriver.WaitUntilNewWindowOpened(command, message).First());
        }

        /// <summary>
        ///     Get current URL query parameters and it's values.
        /// </summary>
        /// <param name="webDriver">Target WebDriver</param>
        /// <returns>Collection of URL parameters names as keys and parameters values as values</returns>
        public static NameValueCollection GetUrlParameters(this IWebDriver webDriver) {
            return HttpUtility.ParseQueryString(new Uri(webDriver.Url).Query);
        }

        /// <summary>
        ///     Retrieve instance of <see cref="IWebDriver"/> wrapped by search context
        /// </summary>
        /// <param name="searchContext">Search content wrapping <see cref="IWebDriver"/> instance</param>
        /// <returns><see cref="IWebDriver"/> instance or null</returns>
        public static IWebDriver ToWebDriver(this ISearchContext searchContext)
        {

            if (searchContext is IWebDriver)
            {
                return (IWebDriver) searchContext;
            }

            if (searchContext is IWrapsDriver)
            {
                return ((IWrapsDriver) searchContext).WrappedDriver;
            }

            if (searchContext is IWrapsElement)
            {
                return (searchContext as IWrapsElement).WrappedElement.ToWebDriver();
            }

            return null;
        }

        /// <summary>
        ///     Retrieve wrapped <see cref="IWebDriver"/> and try to use it as <see cref="IJavaScriptExecutor"/>
        /// </summary>
        /// <param name="searchContext">Search content wrapping <see cref="IWebDriver"/> instance</param>
        /// <returns>WebDriver instance capable of executing JavaScript or null</returns>
        public static IJavaScriptExecutor ToJavaScriptExecutor(this ISearchContext searchContext)
        {

            if (searchContext is IJavaScriptExecutor)
            {
                return (IJavaScriptExecutor)searchContext;
            }

            if (searchContext is IWrapsDriver)
            {
                return (searchContext as IWrapsDriver).WrappedDriver.ToJavaScriptExecutor();
            }

            if (searchContext is IWrapsElement)
            {
                return (searchContext as IWrapsElement).WrappedElement.ToJavaScriptExecutor();
            }

            return null;
        }
    }

}
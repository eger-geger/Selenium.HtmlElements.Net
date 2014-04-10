using System;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

using OpenQA.Selenium;

namespace HtmlElements.Extensions {

    public static class WebDriverExtensions {

        public static void SwitchToLastOpenedWindow(this IWebDriver self) {
            self.SwitchTo().Window(self.WindowHandles.Last());
        }

        public static void WaitUntilNewWindowOpened(this IWebDriver self, Action newWindowTrigger) {
            var initWindowCount = self.WindowHandles.Count;

            newWindowTrigger();

            self.WaitUntil(s => s.WindowHandles.Count > initWindowCount);
        }

        public static void WaitUntilNewWindowOpened(this IWebDriver self, Action newWindowTrigger, TimeSpan timeout) {
            var initWindowCount = self.WindowHandles.Count;

            newWindowTrigger();

            self.WaitUntil(s => s.WindowHandles.Count > initWindowCount, timeout);
        }

        public static bool IsClosed(this IWebDriver driver) {
            try {
                return driver.WindowHandles.Count == 0;
            } catch (Exception) {
                return true;
            }
        }

        public static void SavePageSource(this IWebDriver webDriver, String path) {
            File.WriteAllText(path, webDriver.PageSource);
        }

        public static void SavePageImage(this ITakesScreenshot webDriver, string imagePath, ImageFormat imageFormat) {
            webDriver.GetScreenshot().SaveAsFile(imagePath, imageFormat);
        }

        public static void ScrollTo(this IWebDriver webDriver, long xpos, long ypos) {
            webDriver.ToJavaScriptExecutor().ExecuteScript(String.Format("window.scrollTo({0}, {1});", xpos, ypos));
        }

        public static void ScrollBy(this IWebDriver webDriver, long offsetX, long offsetY) {
            webDriver.ToJavaScriptExecutor().ExecuteScript(String.Format("window.scrollBy({0}, {1});", offsetX, offsetY));
        }

        public static T OpenInNewWindow<T>(this IWebDriver webDriver, Action newWindowTrigger) where T : class {
            webDriver.WaitUntilNewWindowOpened(newWindowTrigger);
            webDriver.SwitchToLastOpenedWindow();
            return webDriver.Activate<T>();
        }

        public static T Activate<T>(this IWebDriver webDriver) where T : class {
            return ElementActivator.Activate<T>(webDriver);
        }

        public static NameValueCollection GetUrlParameters(this IWebDriver self) {
            return HttpUtility.ParseQueryString(new Uri(self.Url).Query);
        }

    }

}
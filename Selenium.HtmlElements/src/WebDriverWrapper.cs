using System;
using System.Collections.ObjectModel;
using System.Text;
using HtmlElements.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements
{
    /// <summary>
    ///     Represents object wrapping web driver. Provides methods for executing JavaScript and retrieving wrapped driver instance. 
    /// </summary>
    public abstract class WebDriverWrapper : ISearchContext, IWrapsDriver, IJavaScriptExecutor
    {
        private readonly IWebDriver _wrappedDriver;

        /// <summary>
        ///     Initializes wrapper converting provided object to WebDriver
        /// </summary>
        /// <param name="webDriverOrWrapper">
        ///     <see cref="IWebDriver"/>, <see cref="IWebElement"/> or anything else 
        ///     which is wrapping WebDriver instance and can be used for locating elements
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="webDriverOrWrapper"/> is not WebDriver and it does not wrap WebDriver
        /// </exception>
        protected WebDriverWrapper(ISearchContext webDriverOrWrapper)
        {
            _wrappedDriver = webDriverOrWrapper.ToWebDriver();

            if (_wrappedDriver == null)
            {
                throw new ArgumentException(
                    "Should be WebDriver or WebDriver wrapper but it isn't", 
                    "webDriverOrWrapper"
                );
            }
        }

        public object ExecuteScript(string script, params object[] args)
        {
            var jsExecutor = WrappedDriver.ToJavaScriptExecutor();

            if (jsExecutor != null)
            {
                return jsExecutor.ExecuteScript(script, args);
            }

            throw new InvalidOperationException(string.Format("[{0}] cannot execute JavaScript", this));
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            var jsExecutor = WrappedDriver.ToJavaScriptExecutor();

            if (jsExecutor != null)
            {
                return jsExecutor.ExecuteAsyncScript(script, args);
            }

            throw new InvalidOperationException(string.Format("[{0}] cannot execute JavaScript", this));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return _wrappedDriver.FindElements(@by);
        }

        public IWebElement FindElement(By @by)
        {
            return _wrappedDriver.FindElement(@by);
        }

        public IWebDriver WrappedDriver
        {
            get { return _wrappedDriver; }
        }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("{0} wrapping", GetType().FullName)
                .AppendLine()
                .AppendLine(_wrappedDriver.ToString().ShiftLinesToRight(4, '.'))
                .ToString();
        }
    }
}
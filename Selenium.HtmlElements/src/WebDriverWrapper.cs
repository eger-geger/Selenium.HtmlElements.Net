using System;
using System.Collections.Generic;
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
        private readonly ISearchContext _webDriverOrWrapper;

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
            _webDriverOrWrapper = webDriverOrWrapper;
        }

        protected IPageObjectFactory PageObjectFactory { get; private set; }

        internal void SetPageObjectFactory(IPageObjectFactory pageObjectFactory)
        {
            PageObjectFactory = pageObjectFactory;
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
            return PageObjectFactory.CreateWebElementList(_webDriverOrWrapper, @by);
        }

        public IWebElement FindElement(By @by)
        {
            return PageObjectFactory.CreateWebElement(_webDriverOrWrapper, @by);
        }

        public TElement FindElement<TElement>(By @by) where TElement:class 
        {
            return PageObjectFactory.CreateWebElement<TElement>(_webDriverOrWrapper, @by);
        }

        public IList<TElement> FindElements<TElement>(By @by) where TElement:class 
        {
            return PageObjectFactory.CreateWebElementList<TElement>(_webDriverOrWrapper, @by);
        }

        public IWebDriver WrappedDriver
        {
            get
            {
                var wrappedDriver = _webDriverOrWrapper.ToWebDriver();

                if (wrappedDriver == null)
                {
                    throw new InvalidOperationException(
                        "Should be WebDriver or WebDriver wrapper but it isn't"
                    );
                };

                return wrappedDriver;
            }
        }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("{0} wrapping", GetType().FullName)
                .AppendLine()
                .AppendLine(_webDriverOrWrapper.ToString().ShiftLinesToRight(2, '.'))
                .ToString();
        }
    }
}
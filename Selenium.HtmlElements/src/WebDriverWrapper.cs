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

        private IPageObjectFactory _pageObjectFactory;

        /// <summary>
        ///     Initializes wrapper converting provided object to WebDriver
        /// </summary>
        /// <param name="webDriverOrWrapper">
        ///     <see cref="IWebDriver"/>, <see cref="IWebElement"/> or anything else 
        ///     which is wrapping WebDriver instance and can be used for locating elements
        /// </param>
        protected WebDriverWrapper(ISearchContext webDriverOrWrapper)
        {
            _webDriverOrWrapper = webDriverOrWrapper;
        }

        /// <summary>
        ///     Gets the page object factory used to initialize current page object instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if page factory was not set during page object initialization.
        /// </exception>
        protected IPageObjectFactory PageObjectFactory
        {
            get
            {
                if (_pageObjectFactory == null)
                {
                    throw new InvalidOperationException(
                        "Attempted to use PageObjectFactory before it was initialized"
                    );
                }

                return _pageObjectFactory;
            }
        }

        internal void SetPageObjectFactory(IPageObjectFactory pageObjectFactory)
        {
            _pageObjectFactory = pageObjectFactory;
        }

        /// <summary>
        ///     Executes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">
        ///     The JavaScript code to execute.
        /// </param>
        /// <param name="args">
        ///     The arguments to the script.
        /// </param>
        /// <returns>
        ///     The value returned by the script.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when wrapped <see cref="IWebDriver"/> cannot be converted to <see cref="IJavaScriptExecutor"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// The method executes JavaScript in the context of the currently selected frame or window. This means that "document" will refer
        /// to the current document. If the script has a return value, then the following steps will be taken:
        /// </para>
        /// <para>
        ///   <list type="bullet">
        ///     <item>
        ///       <description>For an HTML element, this method returns a <see cref="T:OpenQA.Selenium.IWebElement" /></description>
        ///     </item>
        ///     <item>
        ///       <description>For a number, a <see cref="T:System.Int64" /> is returned</description>
        ///     </item>
        ///     <item>
        ///       <description>For a boolean, a <see cref="T:System.Boolean" /> is returned</description>
        ///     </item>
        ///     <item>
        ///       <description>For all other cases a <see cref="T:System.String" /> is returned.</description>
        ///     </item>
        ///     <item>
        ///       <description>For an array, we check the first element, and attempt to return a
        /// <see cref="T:System.Collections.Generic.List`1" /> of that type, following the rules above. Nested lists are not
        /// supported.</description>
        ///     </item>
        ///     <item>
        ///       <description>If the value is null or there is no return value, <see langword="null" /> is returned.</description>
        ///     </item>
        ///   </list>
        /// </para>
        /// <para>
        /// Arguments must be a number (which will be converted to a <see cref="T:System.Int64" />),
        /// a <see cref="T:System.Boolean" />, a <see cref="T:System.String" /> or a <see cref="T:OpenQA.Selenium.IWebElement" />.
        /// An exception will be thrown if the arguments do not meet these criteria.
        /// The arguments will be made available to the JavaScript via the "arguments" magic
        /// variable, as if the function were called via "Function.apply"
        /// </para>
        /// </remarks>
        public object ExecuteScript(string script, params object[] args)
        {
            var jsExecutor = WrappedDriver.ToJavaScriptExecutor();

            if (jsExecutor != null)
            {
                return jsExecutor.ExecuteScript(script, args);
            }

            throw new InvalidOperationException(
                String.Format("[{0}] cannot execute JavaScript", _webDriverOrWrapper)
            );
        }

        /// <summary>
        ///     Executes JavaScript asynchronously in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">
        ///     The JavaScript code to execute.
        /// </param>
        /// <param name="args">
        ///     The arguments to the script.
        /// </param>
        /// <returns>
        ///     The value returned by the script.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when wrapped <see cref="IWebDriver"/> cannot be converted to <see cref="IJavaScriptExecutor"/>.
        /// </exception>
        public object ExecuteAsyncScript(string script, params object[] args)
        {
            var jsExecutor = WrappedDriver.ToJavaScriptExecutor();

            if (jsExecutor != null)
            {
                return jsExecutor.ExecuteAsyncScript(script, args);
            }

            throw new InvalidOperationException(
                String.Format("[{0}] cannot execute JavaScript", _webDriverOrWrapper)
            );
        }

        /// <summary>
        ///     Finds all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current context using the given mechanism.
        /// </summary>
        /// <param name="by">
        ///     The locating mechanism to use.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of all <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
        /// matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return PageObjectFactory.CreateWebElementList(_webDriverOrWrapper, @by);
        }

        /// <summary>
        ///     Finds the first <see cref="T:OpenQA.Selenium.IWebElement" /> using the given method.
        /// </summary>
        /// <param name="by">
        ///     The locating mechanism to use.
        /// </param>
        /// <returns>
        ///     The first matching <see cref="T:OpenQA.Selenium.IWebElement" /> on the current context.
        /// </returns>
        public IWebElement FindElement(By @by)
        {
            return PageObjectFactory.CreateWebElement(_webDriverOrWrapper, @by);
        }

        /// <summary>
        ///     Finds the first <see cref="IWebElement"/> using given method and creates a page object of given type wrapping the element.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        ///     The first matching <see cref="IWebElement" /> on the current context wrapped by page object instance of given type.
        /// </returns>
        public TElement FindElement<TElement>(By @by) where TElement:class 
        {
            return PageObjectFactory.CreateWebElement<TElement>(_webDriverOrWrapper, @by);
        }

        /// <summary>
        ///     Finds all <see cref="IWebElement">IWebElements</see> within the current context using the given mechanism and creates page objects wrapping it.
        /// </summary>
        /// <param name="by">
        ///     The locating mechanism to use.
        /// </param>
        /// <returns>
        ///     A <see cref="IList{T}" /> of all <see cref="IWebElement">WebElements</see> matching the current criteria
        ///     wrapped by page objects instances of given type, or an empty list if nothing matches.
        /// </returns>
        public IList<TElement> FindElements<TElement>(By @by) where TElement:class 
        {
            return PageObjectFactory.CreateWebElementList<TElement>(_webDriverOrWrapper, @by);
        }

        /// <summary>
        ///     Gets the <see cref="T:OpenQA.Selenium.IWebDriver" /> wrapped by current page object instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when wrapped search context cannot be converted to <see cref="IWebDriver"/>.
        /// </exception>
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
                }

                return wrappedDriver;
            }
        }

        /// <summary>
        ///     Describes actual page object type and wrapped search context.
        /// </summary>
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
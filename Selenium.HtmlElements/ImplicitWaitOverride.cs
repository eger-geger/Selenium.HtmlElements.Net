using System;
using OpenQA.Selenium;

namespace HtmlElements
{
    /// <summary>
    ///     Allow to declare block in which implicit wait will be overridden with provided value and reset on exit.
    /// </summary>
    public class ImplicitWaitOverride : IDisposable
    {
        private readonly TimeSpan _defaultImplicitWait;

        private TimeSpan _overriddenImplicitWait;

        private readonly IWebDriver _wrappeDriver;

        /// <summary>
        ///     Create new override for a given browser with provided default value. 
        ///     Does not change implicit wait on it's own. In order to do so use <see cref="ImplicitWaitTimeout"/> property.
        /// </summary>
        /// <param name="wrappeDriver">Driver which implicit wait timeout should be overridden</param>
        /// <param name="defaultImplicitWait">Default implicit wait timeout set when override is being disposed</param>
        public ImplicitWaitOverride(IWebDriver wrappeDriver, TimeSpan defaultImplicitWait)
        {
            _wrappeDriver = wrappeDriver;
            _defaultImplicitWait = defaultImplicitWait;
        }

        /// <summary>
        ///     Create new override for a given browser with provided default value. 
        ///     It actually updates implicit wait setting but it can also be changed later with <see cref="ImplicitWaitTimeout"/> property.
        /// </summary>
        /// <param name="wrappeDriver">Driver which implicit wait timeout should be overridden</param>
        /// <param name="defaultImplicitWait">Default implicit wait timeout set when override is being disposed</param>
        /// <param name="overridenImplicitWait">Implicit wait timeout to be set for a given WebDriver instance</param>
        public ImplicitWaitOverride(IWebDriver wrappeDriver, TimeSpan defaultImplicitWait, TimeSpan overridenImplicitWait)
        {
            _wrappeDriver = wrappeDriver;
            _defaultImplicitWait = defaultImplicitWait;
            ImplicitWaitTimeout = overridenImplicitWait;
        }

        /// <summary>
        ///     Overridden implicit wait timeout set on a browser
        /// </summary>
        public TimeSpan ImplicitWaitTimeout
        {
            get { return _overriddenImplicitWait; }
            set { _wrappeDriver.Manage().Timeouts().ImplicitlyWait(_overriddenImplicitWait = value); }
        }

        /// <summary>
        ///     Reset implicit wait timeout to provided default value
        /// </summary>
        public void Dispose()
        {
            ImplicitWaitTimeout = _defaultImplicitWait;
        }
    }
}
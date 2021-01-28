using System;
using OpenQA.Selenium;

namespace HtmlElements
{
    /// <summary>
    ///     Allow to declare block of code in which implicit WebDriver wait will be overridden with provided value and restored after it.
    /// </summary>
    public class ImplicitWaitOverride : IDisposable, IWrapsDriver
    {
        private readonly TimeSpan _defaultImplicitWait;

        private readonly IWebDriver _wrappedDriver;

        private TimeSpan _overriddenImplicitWait;

        /// <summary>
        ///     Create new override for a given browser with provided default value. 
        ///     Does not change implicit wait on it's own. In order to do so use <see cref="ImplicitWaitTimeout"/> property.
        /// </summary>
        /// <param name="wrappedDriver">Driver which implicit wait timeout should be overridden</param>
        /// <param name="defaultImplicitWait">Default implicit wait timeout set when override is being disposed</param>
        public ImplicitWaitOverride(IWebDriver wrappedDriver, TimeSpan defaultImplicitWait)
        {
            _wrappedDriver = wrappedDriver;
            _defaultImplicitWait = defaultImplicitWait;
        }

        /// <summary>
        ///     Create new override for a given browser with provided default value. 
        ///     It actually updates implicit wait setting but it can also be changed later with <see cref="ImplicitWaitTimeout"/> property.
        /// </summary>
        /// <param name="wrappedDriver">Driver which implicit wait timeout should be overridden</param>
        /// <param name="defaultImplicitWait">Default implicit wait timeout set when override is being disposed</param>
        /// <param name="overridenImplicitWait">Implicit wait timeout to be set for a given WebDriver instance</param>
        public ImplicitWaitOverride(IWebDriver wrappedDriver, TimeSpan defaultImplicitWait,
            TimeSpan overridenImplicitWait)
        {
            _wrappedDriver = wrappedDriver;
            _defaultImplicitWait = defaultImplicitWait;
            ImplicitWaitTimeout = overridenImplicitWait;
        }

        /// <summary>
        ///     Overridden implicit wait timeout set on a browser
        /// </summary>
        public TimeSpan ImplicitWaitTimeout
        {
            get => _overriddenImplicitWait;
            set => _wrappedDriver.Manage().Timeouts().ImplicitWait = _overriddenImplicitWait = value;
        }

        /// <summary>
        ///     Reset implicit wait timeout to provided default value
        /// </summary>
        public void Dispose()
        {
            ImplicitWaitTimeout = _defaultImplicitWait;
        }

        /// <summary>
        ///     Driver instance which timeout get updated
        /// </summary>
        public IWebDriver WrappedDriver => _wrappedDriver;
    }
}
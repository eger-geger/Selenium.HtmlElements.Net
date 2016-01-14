using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Wrappers
{
    /// <summary>
    ///     Describes class which wraps <see cref="IWebDriver"/> and provides additional capabilities
    /// </summary>
    public interface IWedDriverWrapper : IWebDriver, IWrapsDriver
    {
        /// <summary>
        ///     Get persisted timeouts object
        /// </summary>
        ITimeoutsWrapper TimeoutsWrapper { get; }

        /// <summary>
        ///     Implicit wait used by <see cref="ResetImplicitWait"/>
        /// </summary>
        TimeSpan DefaultImplicitWait { get; }

        /// <summary>
        ///     Reset implicit wait to some default value or throw exception when it is impossible
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when default implicit wait value is not available
        /// </exception>
        void ResetImplicitWait();
    }
}
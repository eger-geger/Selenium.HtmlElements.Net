using System;
using OpenQA.Selenium;

namespace HtmlElements.Wrappers
{
    /// <summary>
    ///     WebDriver timeouts which can be not only set but read as well
    /// </summary>
    public interface ITimeoutsWrapper : ITimeouts
    {
        /// <summary>
        ///     Synonym of <see cref="ITimeouts.ImplicitlyWait"/>
        /// </summary>
        TimeSpan ImplicitWait { get; set; }

        /// <summary>
        ///     Synonym of <see cref="ITimeouts.SetPageLoadTimeout"/>
        /// </summary>
        TimeSpan PageLoad { get; set; }

        /// <summary>
        ///     Synonym of <see cref="ITimeouts.SetScriptTimeout"/>
        /// </summary>
        TimeSpan ScriptExecution { get; set; }
    }
}
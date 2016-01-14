using System;
using OpenQA.Selenium;

namespace HtmlElements.Wrappers
{
    public class TimeoutsWrapper : ITimeoutsWrapper
    {
        private readonly ITimeouts _timeouts;

        private TimeSpan _implicitWaitTimeout = TimeSpan.MinValue;
        private TimeSpan _pageLoadTimeout = TimeSpan.MinValue;
        private TimeSpan _scriptTimeout = TimeSpan.MinValue;

        public TimeoutsWrapper(ITimeouts timeouts)
        {
            _timeouts = timeouts;
        }

        public TimeSpan ImplicitWait
        {
            get { return _implicitWaitTimeout; }

            set { _timeouts.ImplicitlyWait(_implicitWaitTimeout = value); }
        }

        public TimeSpan PageLoad
        {
            get { return _pageLoadTimeout; }

            set { _timeouts.SetPageLoadTimeout(_pageLoadTimeout = value); }
        }

        public TimeSpan ScriptExecution
        {
            get { return _scriptTimeout; }

            set { _timeouts.SetScriptTimeout(_scriptTimeout = value); }
        }

        public ITimeouts ImplicitlyWait(TimeSpan timeToWait)
        {
            ImplicitWait = timeToWait;

            return this;
        }

        public ITimeouts SetScriptTimeout(TimeSpan timeToWait)
        {
            ScriptExecution = timeToWait;

            return this;
        }

        public ITimeouts SetPageLoadTimeout(TimeSpan timeToWait)
        {
            PageLoad = timeToWait;

            return this;
        }
    }
}
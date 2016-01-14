using System;
using OpenQA.Selenium;

namespace HtmlElements.Wrappers
{
    public class OptionsWrapper : IOptions
    {
        private readonly Lazy<ITimeoutsWrapper> _lazyTimeouts;
        private readonly IOptions _options;

        public OptionsWrapper(IOptions options)
        {
            _options = options;

            _lazyTimeouts = new Lazy<ITimeoutsWrapper>(() => new TimeoutsWrapper(_options.Timeouts()));
        }

        public ITimeoutsWrapper TimeoutsWrapper
        {
            get { return _lazyTimeouts.Value; }
        }

        public ITimeouts Timeouts()
        {
            return _lazyTimeouts.Value;
        }

        public ICookieJar Cookies
        {
            get { return _options.Cookies; }
        }

        public IWindow Window
        {
            get { return _options.Window; }
        }
    }
}
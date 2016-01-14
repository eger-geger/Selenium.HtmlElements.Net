using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements.Wrappers
{
    public class WebDriverWrapper : IWedDriverWrapper
    {
        private readonly Lazy<OptionsWrapper> _lazyOptions;

        private readonly IWebDriver _wrappedDriver;

        private readonly Nullable<TimeSpan> _defaultImplicitWait;

        public WebDriverWrapper(IWebDriver wrappedDriver, TimeSpan defaultImplicitWait) : this (wrappedDriver)
        {
            _defaultImplicitWait = defaultImplicitWait;
        }

        public WebDriverWrapper(IWebDriver wrappedDriver)
        {
            _wrappedDriver = wrappedDriver;

            _lazyOptions = new Lazy<OptionsWrapper>(() => new OptionsWrapper(_wrappedDriver.Manage()));
        }

        public ITimeoutsWrapper TimeoutsWrapper
        {
            get { return _lazyOptions.Value.TimeoutsWrapper; }
        }

        public TimeSpan DefaultImplicitWait
        {
            get { return _defaultImplicitWait ?? TimeoutsWrapper.ImplicitWait; }
        }

        public void ResetImplicitWait()
        {
            if (DefaultImplicitWait < TimeSpan.Zero)
            {
                throw new InvalidOperationException(
                    String.Format("Cannot reset implicit wait because default value is invalid: {0}", DefaultImplicitWait)
                );
            }

            TimeoutsWrapper.ImplicitWait = DefaultImplicitWait;
        }

        public IWebElement FindElement(By @by)
        {
            return _wrappedDriver.FindElement(@by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return _wrappedDriver.FindElements(@by);
        }

        public void Dispose()
        {
            _wrappedDriver.Dispose();
        }

        public void Close()
        {
            _wrappedDriver.Close();
        }

        public void Quit()
        {
            _wrappedDriver.Quit();
        }

        public IOptions Manage()
        {
            return _lazyOptions.Value;
        }

        public INavigation Navigate()
        {
            return _wrappedDriver.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return _wrappedDriver.SwitchTo();
        }

        public String Url
        {
            get { return _wrappedDriver.Url; }
            set { _wrappedDriver.Url = value; }
        }

        public String Title
        {
            get { return _wrappedDriver.Title; }
        }

        public String PageSource
        {
            get { return _wrappedDriver.PageSource; }
        }

        public String CurrentWindowHandle
        {
            get { return _wrappedDriver.CurrentWindowHandle; }
        }

        public ReadOnlyCollection<String> WindowHandles
        {
            get { return _wrappedDriver.WindowHandles; }
        }

        public IWebDriver WrappedDriver
        {
            get { return _wrappedDriver; }
        }
    }
}
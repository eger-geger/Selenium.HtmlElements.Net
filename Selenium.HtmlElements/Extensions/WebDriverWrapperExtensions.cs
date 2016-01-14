using System;
using HtmlElements.Wrappers;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Extensions
{
    public static class WebDriverWrapperExtensions
    {
        public static IWedDriverWrapper GetWebDriverWrapper(this Object something)
        {
            if (something is IWedDriverWrapper)
            {
                return something as IWedDriverWrapper;
            }

            if (something is IWrapsDriver)
            {
                return GetWebDriverWrapper((something as IWrapsDriver).WrappedDriver);
            }

            if (something is ISearchContext)
            {
                return GetWebDriverWrapper((something as ISearchContext).ToWebDriver());
            }

            return null;
        }
    }
}
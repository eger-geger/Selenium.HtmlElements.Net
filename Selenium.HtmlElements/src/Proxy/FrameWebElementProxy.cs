using System;
using System.Collections.ObjectModel;
using HtmlElements.Extensions;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;

namespace HtmlElements.Proxy
{
    internal class FrameWebElementProxy : WebElementProxy
    {
        public FrameWebElementProxy(ILoader<IWebElement> loader) : base(loader)
        {
        }

        private TResult ExecuteInFrame<TResult>(Func<IWebDriver, TResult> function)
        {
            var webDriver = Loader.SearchContext.ToWebDriver();

            if (webDriver == null)
            {
                throw new InvalidOperationException(
                    string.Format("Unable switch to frame because frame element does not wraps {0}", typeof(IWebDriver))
                );
            }

            return function(webDriver);
        }

        public override IWebElement FindElement(By @by)
        {
            return ExecuteInFrame(wd => wd.FindElement(@by));
        }

        public override ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return ExecuteInFrame(wd => wd.FindElements(@by));
        }
    }
}

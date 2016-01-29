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
            return Execute(frame =>
            {
                if (frame == null)
                {
                    throw new ArgumentNullException("frame");
                }

                var webDriver = frame.ToWebDriver();

                if (webDriver == null)
                {
                    throw new InvalidOperationException(
                        String.Format("Unable switch to frame because frame element does not wraps {0}", typeof (IWebDriver))
                    );
                }

                var rawWebElement = frame.ToRawWebElement();

                if (rawWebElement == null)
                {
                    throw new InvalidOperationException(
                        "Unable to retrieve raw web element from frame element"
                    );
                }

                return function(webDriver);
            });
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

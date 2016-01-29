using System;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;

namespace HtmlElements.Proxy
{
    internal class WebElementProxy : AbstractWebElementProxy
    {
        public WebElementProxy(ILoader<IWebElement> loader) : base(loader)
        {
        }

        protected override void Execute(Action<IWebElement> action)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    action(Loader.Load());
                }
                catch (StaleElementReferenceException)
                {
                    Loader.Reset();

                    if (i == 4)
                    {
                        throw;
                    }

                    continue;
                }

                break;
            }
        }
    }
}
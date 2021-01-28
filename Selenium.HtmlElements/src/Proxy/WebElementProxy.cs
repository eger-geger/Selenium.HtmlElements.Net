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

        private bool IsStaleElementReferenceException(Exception ex)
        {
            return ex is StaleElementReferenceException ||
                   (ex is InvalidOperationException &&
                    ex.Message.Contains("Element does not exist in cache (status: 10)"));
        }

        protected override void Execute(Action<IWebElement> action)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    action(Loader.Load());
                }
                catch (Exception ex)
                {
                    if (!IsStaleElementReferenceException(ex))
                    {
                        throw;
                    }

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
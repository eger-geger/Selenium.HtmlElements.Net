using System;
using System.Reflection;
using Castle.DynamicProxy;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;

namespace HtmlElements.Proxy
{
    internal class WebElementInterceptor : IInterceptor
    {
        private const Int32 RetryCount = 5;

        private readonly ILoader<IWebElement> _loader;

        public WebElementInterceptor(ILoader<IWebElement> loader)
        {
            _loader = loader;
        }

        public void Intercept(IInvocation invocation)
        {
            for (var i = 0; i < RetryCount; i++)
            {
                try
                {
                    invocation.ReturnValue = invocation.Method.Invoke(
                        _loader.Load(), invocation.Arguments
                    );
                }
                catch (TargetInvocationException exception)
                {
                    _loader.Reset();

                    if (exception.InnerException is StaleElementReferenceException)
                    {
                        continue;
                    }

                    throw;
                }

                break;
            }
        }
    }
}
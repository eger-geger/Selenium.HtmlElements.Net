using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Castle.DynamicProxy;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;

namespace HtmlElements.Proxy
{
    internal class ProxyFactory : IProxyFactory
    {
        private readonly ProxyGenerator _proxyFactory = new ProxyGenerator();

        public IList<IWebElement> CreateWebElementListProxy(ILoader<ReadOnlyCollection<IWebElement>> loader)
        {
            throw new NotImplementedException();
        }

        public IWebElement CreateWebElementProxy(ILoader<IWebElement> elementLoader)
        {
            return _proxyFactory.CreateInterfaceProxyWithoutTarget<IWebElement>(
                new WebElementInterceptor(elementLoader)
            );
        }
    }
}
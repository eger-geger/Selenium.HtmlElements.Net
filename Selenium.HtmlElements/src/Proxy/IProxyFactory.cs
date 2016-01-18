using System.Collections.Generic;
using System.Collections.ObjectModel;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;

namespace HtmlElements.Proxy
{
    internal interface IProxyFactory
    {
        IWebElement CreateWebElementProxy(ILoader<IWebElement> loader);

        IList<IWebElement> CreateWebElementListProxy(ILoader<ReadOnlyCollection<IWebElement>> loader);
    }
}

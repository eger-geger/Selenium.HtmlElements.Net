using System;
using System.Reflection;
using System.Text;
using HtmlElements.Elements;
using HtmlElements.Extensions;
using HtmlElements.LazyLoad;
using HtmlElements.Locators;
using HtmlElements.Proxy;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace HtmlElements
{
    /// <summary>
    ///     Default page object implementation creating lazy loading proxies for every web element or list of web elements.
    ///     It can't handle multiple <see cref="FindsByAttribute"/> attributes as well as <see cref="FindsBySequenceAttribute"/> and <see cref="FindsByAllAttribute"/>. 
    /// 
    ///     It supports <see cref="CacheLookupAttribute"/> for elements and element lists by using same raw web element once it have been found.
    ///     It also supports smart frames (derived from <see cref="HtmlFrame"/> class) which can host nested elements and automatically switch context.
    /// 
    ///     Factory requires nested page objects to have default constructor or constructor accepting <see cref="IWebElement"/> as a single argument
    ///     or <see cref="IWebElement"/> as first argument and <see cref="IPageObjectFactory"/> as second.
    /// 
    ///     When <see cref="IPageObjectFactory.Create{TPageObject}"/> is called directly it will pass instance of search context being provided as first constructor 
    ///     argument and itself as second (if needed). It also can use default constructor.
    /// </summary>
    public class PageObjectFactory : AbstractPageObjectFactory
    {
        private readonly ILoaderFactory _loaderFactory;
        private readonly IProxyFactory _proxyFactory;

        /// <summary>
        ///     Creates page factory instance using <see cref="ProxyFactory"/> for creating lazy loading error handling proxies
        ///     and <see cref="LoaderFactory"/> for creating lazy loaded elements and element lists.
        /// </summary>
        public PageObjectFactory()
        {
            _proxyFactory = new ProxyFactory();
            _loaderFactory = new LoaderFactory(this, _proxyFactory);
        }

        /// <summary>
        ///     Creates page factory instance using provided <paramref name="proxyFactory"/> for creating proxies
        ///     and <paramref name="loaderFactory"/> for creating lazy elements and list of elements.
        /// </summary>
        /// <param name="proxyFactory"></param>
        /// <param name="loaderFactory"></param>
        public PageObjectFactory(IProxyFactory proxyFactory, ILoaderFactory loaderFactory)
        {
            if (proxyFactory == null)
            {
                throw new ArgumentNullException("proxyFactory");
            }

            if (loaderFactory == null)
            {
                throw new ArgumentNullException("loaderFactory");
            }

            _proxyFactory = proxyFactory;
            _loaderFactory = loaderFactory;
        }

        protected override Object CreateMemberInstance(Type memberType, MemberInfo memberInfo, ISearchContext searchContext)
        {
            var locator = CreateElementLocator(memberInfo);

            var isCached = memberInfo.IsDefined(typeof (CacheLookupAttribute), true);

            if (memberType.IsWebElement())
            {
                ILoader<IWebElement> elementLoader = _loaderFactory.CreateElementLoader(searchContext, locator, isCached);

                IWebElement elementProxy = 
                    typeof(HtmlFrame).IsAssignableFrom(memberType)
                        ? _proxyFactory.CreateFrameProxy(elementLoader)
                        : _proxyFactory.CreateElementProxy(elementLoader);

                if (typeof (IWebElement) == memberType || typeof (IHtmlElement) == memberType)
                {
                    return new HtmlElement(elementProxy);
                }

                return Create(memberType, elementProxy);
            }

            if (memberType.IsWebElementList())
            {
                var genericArguments = memberType.GetGenericArguments();

                if (genericArguments.Length == 0)
                {
                    return null;
                }

                var elementType = genericArguments[0];

                if (elementType == typeof (IWebElement) || elementType == typeof (IHtmlElement))
                {
                    elementType = typeof (HtmlElement);
                }

                return _proxyFactory.CreateListProxy(
                    elementType, _loaderFactory.CreateListLoader(elementType, searchContext, locator, isCached)
                );
            }

            return null;
        }

        private By CreateElementLocator(MemberInfo memberInfo)
        {
            var attributes = memberInfo.GetCustomAttributes(typeof (FindsByAttribute), true);

            if (attributes.Length == 0)
            {
                return ByFactory.Create(How.Id, memberInfo.Name);
            }

            if (attributes.Length > 1)
            {
                var errorBuilder = new StringBuilder();

                errorBuilder.AppendFormat(
                    "Multiple attributes [{0}] found on [{1}] which is member of [{2}] class.",
                    typeof (FindsByAttribute), memberInfo.Name, memberInfo.DeclaringType);

                errorBuilder.AppendFormat(
                    "Having multiple attribute for a single class member is not supported by [{0}].", this);

                throw new ArgumentException(errorBuilder.ToString(), "memberInfo");
            }

            return ByFactory.Create(attributes[0] as FindsByAttribute);
        }

        protected override Object CreatePageObjectInstance(Type pageObjectType, ISearchContext searchContext)
        {
            try
            {
                return Activator.CreateInstance(pageObjectType, searchContext, this);
            }
            catch (MissingMethodException)
            {
                //ignore and proceed
            }

            try
            {
                return Activator.CreateInstance(pageObjectType, searchContext);
            }
            catch (MissingMethodException)
            {
                //ignore and proceed
            }

            return Activator.CreateInstance(pageObjectType);
        }
    }
}
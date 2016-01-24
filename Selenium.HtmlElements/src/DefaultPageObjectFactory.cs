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
    public class DefaultPageObjectFactory : AbstractPageObjectFactory
    {
        private readonly ProxyFactory _proxyFactory = new ProxyFactory();

        private readonly ElementLoaderFactory _loaderFactory;

        public DefaultPageObjectFactory()
        {
            _loaderFactory = new ElementLoaderFactory(this, _proxyFactory);
        }

        protected override Object CreateMemberInstance(Type memberType, MemberInfo memberInfo, ISearchContext searchContext)
        {
            var locator = CreateElementLocator(memberInfo);

            var isCached = memberInfo.IsDefined(typeof (CacheLookupAttribute), true);

            if (memberType.IsWebElement())
            {
                var elementProxy = _proxyFactory.CreateWebElementProxy(
                    _loaderFactory.CreateElementLoader(searchContext, locator, isCached)
                );

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

                return Activator.CreateInstance(
                    typeof (ElementListProxy<>).MakeGenericType(elementType), 
                    _loaderFactory.CreateTypedListLoader(elementType, searchContext, locator, isCached)
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
                return Activator.CreateInstance(pageObjectType, searchContext);
            }
            catch (MissingMethodException)
            {
                return Activator.CreateInstance(pageObjectType);
            }
        }
    }
}
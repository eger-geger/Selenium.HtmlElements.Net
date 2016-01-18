using System;
using System.Collections.ObjectModel;
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
    public class PageObjectFactory : IPageObjectFactory
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.DeclaredOnly
                                                  | System.Reflection.BindingFlags.Instance
                                                  | System.Reflection.BindingFlags.Public
                                                  | System.Reflection.BindingFlags.NonPublic;

        private readonly ProxyFactory _proxyFactory = new ProxyFactory();

        public object Create(Type pageObjecType, ISearchContext searchContext)
        {
            var pageObject = CreateInstance(pageObjecType, searchContext);

            if (pageObject is IPageObjectFactoryWrapper)
            {
                (pageObject as IPageObjectFactoryWrapper).PageObjectFactory = this;
            }

            Init(pageObjecType, searchContext);

            return pageObject;
        }

        public TPageObject Create<TPageObject>(ISearchContext searchContext)
        {
            return (TPageObject) Create(typeof (TPageObject), searchContext);
        }

        public void Init(object pageObject, ISearchContext searchContext)
        {
            if (pageObject == null)
            {
                throw new ArgumentNullException(nameof(pageObject));
            }

            if (searchContext == null)
            {
                throw new ArgumentNullException(nameof(searchContext));
            }

            var pageObjectType = pageObject.GetType();

            foreach (var fieldInfo in pageObjectType.GetOwnAndInheritedFields(BindingFlags))
            {
                if (fieldInfo.GetValue(pageObject) != null)
                {
                    continue;
                }

                fieldInfo.SetValue(pageObject, CreateMember(fieldInfo.FieldType, fieldInfo, searchContext));
            }

            foreach (var propertyInfo in pageObjectType.GetOwnAndInheritedProperties(BindingFlags))
            {
                if (propertyInfo.GetValue(pageObject, null) != null)
                {
                    continue;
                }

                propertyInfo.SetValue(pageObject, CreateMember(propertyInfo.PropertyType, propertyInfo, searchContext),
                    null);
            }
        }

        public void Init(SearchContextWrapper pageObject)
        {
            Init(pageObject, pageObject);
        }

        private Object CreateMember(Type memberType, MemberInfo memberInfo, ISearchContext searchContext)
        {
            var locator = CreateLocator(memberInfo);

            var isCached = memberInfo.IsDefined(typeof (CacheLookupAttribute), true);

            if (memberType.IsWebElement())
            {
                var elementProxy = isCached
                    ? _proxyFactory.CreateWebElementProxy(new CachingWebElementLoader(searchContext, locator))
                    : _proxyFactory.CreateWebElementProxy(new TransparentWebElementLoader(searchContext, locator));

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

                var genericArgument = genericArguments[0];

                if (genericArgument == typeof (IWebElement) || genericArgument == typeof (IHtmlElement))
                {
                    genericArgument = typeof (HtmlElement);
                }

                ILoader<ReadOnlyCollection<IWebElement>> listLoader;

                if (isCached)
                {
                    listLoader = new CachingWebElementListLoader(searchContext, locator);
                }
                else
                {
                    listLoader = new TransparentWebElementListLoader(locator, searchContext);
                }

                var loader = isCached
                    ? CreateGenericInstance(typeof (CachingTypedListLoader<>), genericArgument, listLoader, this,
                        _proxyFactory)
                    : CreateGenericInstance(typeof (TransparentTypedListLoader<>), genericArgument, listLoader, this,
                        _proxyFactory);

                return Activator.CreateInstance(typeof (ElementListProxy<>).MakeGenericType(genericArgument), loader);
            }

            return null;
        }

        protected virtual By CreateLocator(MemberInfo memberInfo)
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

                throw new ArgumentException(errorBuilder.ToString(), nameof(memberInfo));
            }

            return ByFactory.Create(attributes[0] as FindsByAttribute);
        }

        private static Object CreateGenericInstance(Type type, Type generic, params Object[] arguments)
        {
            return Activator.CreateInstance(type.MakeGenericType(generic), arguments);
        }

        protected virtual Object CreateInstance(Type type, ISearchContext searchContext)
        {
            try
            {
                return Activator.CreateInstance(type, searchContext);
            }
            catch (MissingMethodException)
            {
                return Activator.CreateInstance(type);
            }
        }
    }
}
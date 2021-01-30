using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using HtmlElements.Elements;
using HtmlElements.Extensions;
using HtmlElements.LazyLoad;
using HtmlElements.Locators;
using HtmlElements.Proxy;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace HtmlElements
{
    /// <summary>
    /// Default page object implementation creating lazy loading proxies for every web element or list
    /// of web elements. It can't handle multiple <see cref="FindsByAttribute"/> attributes as well as
    /// <see cref="FindsBySequenceAttribute"/> and <see cref="FindsByAllAttribute"/>.
    ///
    /// It supports <see cref="CacheLookupAttribute"/> for elements and element lists by using same
    /// raw web element once it have been found. It also supports smart frames (derived from <see
    /// cref="HtmlFrame"/> class) which can host nested elements and automatically switch context.
    ///
    /// Factory requires nested page objects to have default constructor or constructor accepting <see
    /// cref="IWebElement"/> as a single argument or <see cref="IWebElement"/> as first argument and
    /// <see cref="IPageObjectFactory"/> as second.
    ///
    /// When <see cref="IPageObjectFactory.Create{TPageObject}"/> is called directly it will pass
    /// instance of search context being provided as first constructor argument and itself as second
    /// (if needed). It also can use default constructor.
    /// </summary>
    public class PageObjectFactory : AbstractPageObjectFactory
    {
        private readonly ILoaderFactory _loaderFactory;

        private readonly IProxyFactory _proxyFactory;

        /// <summary>
        /// Creates page factory instance using <see cref="ProxyFactory"/> for creating lazy loading
        /// error handling proxies and <see cref="LoaderFactory"/> for creating lazy loaded elements and
        /// element lists.
        /// </summary>
        public PageObjectFactory()
        {
            _proxyFactory = new ProxyFactory();
            _loaderFactory = new LoaderFactory(this, _proxyFactory);
        }

        /// <summary>
        /// Creates page factory instance using provided <paramref name="proxyFactory"/> for creating
        /// proxies and <paramref name="loaderFactory"/> for creating lazy elements and list of elements.
        /// </summary>
        /// <param name="proxyFactory">Factory creating WebElement and WebElement list proxies.</param>
        /// <param name="loaderFactory">Factory creating WebElement and WebElement list loaders.</param>
        public PageObjectFactory(IProxyFactory proxyFactory, ILoaderFactory loaderFactory)
        {
            _proxyFactory = proxyFactory ?? throw new ArgumentNullException(nameof(proxyFactory));
            _loaderFactory = loaderFactory ?? throw new ArgumentNullException(nameof(loaderFactory));
        }

        /// <summary>
        /// Creates lazy loaded WebElement found with provided locator in given search context.
        /// </summary>
        /// <param name="searchContext">Context used for finding element.</param>
        /// <param name="locator">Element locator to use for finding element.</param>
        /// <returns>Lazy loaded WebElement found in given search context with provided locator.</returns>
        public override IWebElement CreateWebElement(ISearchContext searchContext, By locator)
        {
            return _proxyFactory.CreateWebElementProxy(
                _loaderFactory.CreateElementLoader(searchContext, locator, true));
        }

        /// <summary>
        /// Creates and initializes page object of a given type and all nested page objects using
        /// WebElement found within given context by provided locator.
        /// </summary>
        /// <typeparam name="TPageObject">Page object class.</typeparam>
        /// <param name="searchContext">
        /// Parent context used for finding the element used as page element root.
        /// </param>
        /// <param name="locator">
        /// Locator used for finding underlying WebElement used as page element root.
        /// </param>
        /// <returns>
        /// Fully initialized page object using WebElement found in <paramref name="searchContext"/>
        /// with <paramref name="locator"/> for finding nested elements.
        /// </returns>
        public override TPageObject CreateWebElement<TPageObject>(ISearchContext searchContext, By locator)
        {
            return CreateWebElement(typeof(TPageObject), searchContext, locator, true) as TPageObject;
        }

        private object CreateWebElement(Type elementType, ISearchContext searchContext, By locator, bool cached)
        {
            var elementLoader = _loaderFactory.CreateElementLoader(searchContext, locator, cached);

            var elementProxy =
                typeof(HtmlFrame).IsAssignableFrom(elementType)
                    ? _proxyFactory.CreateFrameProxy(elementLoader)
                    : _proxyFactory.CreateWebElementProxy(elementLoader);

            return Create(elementType, elementProxy);
        }

        /// <summary>
        /// Creates lazy loaded list of WebElements found with provided locator in given search context
        /// </summary>
        /// <param name="searchContext">Context used for finding elements</param>
        /// <param name="locator">Element locator to use for finding elements</param>
        /// <returns>Lazy loaded list of WebElements</returns>
        public override ReadOnlyCollection<IWebElement> CreateWebElementList(ISearchContext searchContext, By locator)
        {
            return new(
                _proxyFactory.CreateListProxy(_loaderFactory.CreateElementListLoader(searchContext, locator, true))
            );
        }

        /// <summary>
        /// Creates and initializes list of page elements and it's nested page objects.
        /// </summary>
        /// <typeparam name="TPageObject">The type of the page object.</typeparam>
        /// <param name="searchContext">The search context used for finding underlying WebElements.</param>
        /// <param name="locator">The locator used for finding underlying WebElements.</param>
        /// <returns>
        /// List of initialized page objects wrapping elements found in <paramref name="searchContext"/>
        /// with <paramref name="locator"/>.
        /// </returns>
        public override IList<TPageObject> CreateWebElementList<TPageObject>(ISearchContext searchContext, By locator)
        {
            return CreateWebElementList(typeof(TPageObject), searchContext, locator, true) as IList<TPageObject>;
        }

        private object CreateWebElementList(Type elementType, ISearchContext searchContext, By locator, bool isCached)
        {
            return _proxyFactory.CreateListProxy(
                elementType, _loaderFactory.CreateListLoader(elementType, searchContext, locator, isCached)
            );
        }

        /// <summary>
        /// Creates value assigned to page object member (field or property).
        /// </summary>
        /// <param name="memberType">Declared type of property or field.</param>
        /// <param name="memberInfo">Field or property meta information.</param>
        /// <param name="searchContext">Parent page object context.</param>
        /// <returns>Initialized field or property value or null.</returns>
        protected override object CreateMemberInstance(
            Type memberType,
            MemberInfo memberInfo,
            ISearchContext searchContext
        )
        {
            return memberType switch
            {
                { } elementType when elementType.IsWebElement() => CreateWebElement(
                    elementType,
                    searchContext,
                    CreateElementLocator(memberInfo, memberType),
                    memberInfo.IsDefined(typeof(CacheLookupAttribute), true)
                    || memberType.IsDefined(typeof(CacheLookupAttribute), true)
                ),

                { } listType when listType.IsWebElementList(out var elementType) => CreateWebElementList(
                    elementType,
                    searchContext,
                    CreateElementLocator(memberInfo, elementType),
                    memberInfo.IsDefined(typeof(CacheLookupAttribute), true)
                    || elementType.IsDefined(typeof(CacheLookupAttribute), true)
                ),

                _ => null
            };
        }

        private By CreateElementLocator(MemberInfo memberInfo, Type webElementType)
        {
            var memberAttr = GetSingleMemberAttrOrDefault(memberInfo);
            var typeAttr = GetSingleTypeAttrOrDefault(webElementType);

            return memberAttr != null
                ? ByFactory.Create(memberAttr)
                : typeAttr != null
                    ? ByFactory.Create(typeAttr)
                    : ByFactory.Create(How.Id, memberInfo.Name);
        }

        private static ElementLocatorAttribute GetSingleTypeAttrOrDefault(Type elementType)
        {
            var attrs = elementType
                .GetCustomAttributes(typeof(ElementLocatorAttribute), true)
                .Cast<ElementLocatorAttribute>()
                .ToArray();

            return attrs.Length == 1 ? attrs[0] : null;
        }

        private FindsByAttribute GetSingleMemberAttrOrDefault(MemberInfo memberInfo)
        {
            var attrs = memberInfo
                .GetCustomAttributes(typeof(FindsByAttribute), true)
                .Cast<FindsByAttribute>()
                .ToArray();

            return attrs switch
            {
                {Length: 0} => null,
                {Length: 1} => attrs[0],
                _ => throw new ArgumentException(BuildMultipleMemberAttrError(memberInfo))
            };
        }

        private string BuildMultipleMemberAttrError(MemberInfo memberInfo)
        {
            return new StringBuilder()
                .AppendFormat("Multiple {0} found on {1} (member of {2})",
                    typeof(FindsByAttribute), memberInfo.Name, memberInfo.DeclaringType)
                .AppendFormat("which is not supported by {0}.", this)
                .ToString();
        }

        /// <summary>
        /// Creates page object using two-arguments constructor ( <see cref="ISearchContext"/> and <see
        /// cref="IPageObjectFactory"/>) or single argument constructor ( <see cref="ISearchContext"/>)
        /// or default constructor.
        /// </summary>
        /// <param name="pageObjectType">Type of page object being initialized.</param>
        /// <param name="searchContext">
        /// Optional constructor argument representing search context being wrapped. It could be <see
        /// cref="IWebElement"/> or <see cref="IWebDriver"/> instance or other page object.
        /// </param>
        /// <returns>New instance of given type.</returns>
        protected override object CreatePageObjectInstance(Type pageObjectType, ISearchContext searchContext)
        {
            if (pageObjectType == typeof(IWebElement) || pageObjectType == typeof(IHtmlElement))
            {
                return new HtmlElement(searchContext as IWebElement);
            }

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
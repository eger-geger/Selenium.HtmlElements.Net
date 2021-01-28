using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements
{
    /// <summary>
    ///     Implements <see cref="IPageObjectFactory" /> contract and delegates responsibility for creating actual page object
    ///     and initializing it's members to subclasses.
    ///     It class takes care of locating suitable for initialization members and assigning value to it.
    ///     It looks for all instance fields and properties which are web elements or web element lists and have not been
    ///     assigned value yet.
    ///     Both fields and properties can be private protected or public, but in order to be initialized
    ///     fields should not be marked
    ///     <value>readonly</value>
    ///     and properties should have setters.
    /// </summary>
    public abstract class AbstractPageObjectFactory : IPageObjectFactory
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.DeclaredOnly
                                                  | System.Reflection.BindingFlags.Instance
                                                  | System.Reflection.BindingFlags.Public
                                                  | System.Reflection.BindingFlags.NonPublic;

        /// <summary>
        ///     Creates and initializes page object of a given type and all nested page objects.
        /// </summary>
        /// <param name="pageObjectType">Page object type</param>
        /// <param name="searchContext">Context used for finding elements</param>
        /// <returns>Fully initialized page object</returns>
        public object Create(Type pageObjectType, ISearchContext searchContext)
        {
            var pageObject = CreatePageObjectInstance(pageObjectType, searchContext);

            Init(pageObject, searchContext);

            return pageObject;
        }

        /// <summary>
        ///     Creates and initializes page object of a given type and all nested page objects.
        /// </summary>
        /// <typeparam name="TPageObject">Page object type</typeparam>
        /// <param name="searchContext">Context used for finding elements</param>
        /// <returns>Fully initialized page object</returns>
        public TPageObject Create<TPageObject>(ISearchContext searchContext)
        {
            return (TPageObject) Create(typeof (TPageObject), searchContext);
        }

        /// <summary>
        ///     Initialize web elements in given page object instance
        /// </summary>
        /// <param name="pageObject">Not initialized page object</param>
        /// <param name="searchContext">Context used for finding elements</param>
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

            if (pageObject is WebDriverWrapper)
            {
                (pageObject as WebDriverWrapper).SetPageObjectFactory(this);
            }

            foreach (var fieldInfo in pageObjectType.GetOwnAndInheritedFields(BindingFlags))
            {
                if (fieldInfo.GetValue(pageObject) != null)
                {
                    continue;
                }

                fieldInfo.SetValue(pageObject, CreateMemberInstance(fieldInfo.FieldType, fieldInfo, searchContext));
            }

            foreach (var propertyInfo in pageObjectType.GetOwnAndInheritedProperties(BindingFlags))
            {
                if (propertyInfo.GetValue(pageObject, null) != null)
                {
                    continue;
                }

                propertyInfo.SetValue(pageObject, CreateMemberInstance(propertyInfo.PropertyType, propertyInfo, searchContext), null);
            }
        }

        /// <summary>
        ///     Initialize page object using it's wrapped context
        /// </summary>
        /// <param name="pageObject">Page object instance to be initialized</param>
        public void Init(WebDriverWrapper pageObject)
        {
            Init(pageObject, pageObject);
        }

        /// <summary>
        ///     Creates WebElement found with provided locator in given search context
        /// </summary>
        /// <param name="searchContext">
        ///     Context used for finding element
        /// </param>
        /// <param name="locator">
        ///     Element locator to use for finding element
        /// </param>
        /// <returns>
        ///     Custom WebElement found in given search context with provided locator
        /// </returns>
        public abstract IWebElement CreateWebElement(ISearchContext searchContext, By locator);

        /// <summary>
        ///     Creates list of WebElements found with provided locator in given search context
        /// </summary>
        /// <param name="searchContext">
        ///     Context used for finding elements
        /// </param>
        /// <param name="locator">
        ///     Element locator to use for finding elements
        /// </param>
        /// <returns>
        ///     Custom list of WebElements
        /// </returns>
        public abstract ReadOnlyCollection<IWebElement> CreateWebElementList(ISearchContext searchContext, By locator);

        /// <summary>
        ///     Creates and initializes page object of a given type and all nested page objects.
        /// </summary>
        /// <typeparam name="TPageObject">
        ///     Page object class.
        /// </typeparam>
        /// <param name="searchContext">
        ///     Parent context used for finding the element used as page element root.
        /// </param>
        /// <param name="locator">
        ///     Locator used for finding underlying WebElement used as page element root.
        /// </param>
        /// <returns>
        ///     Fully initialized page object using WebElement found in <paramref name="searchContext"/> with <paramref name="locator"/> for finding nested elements.
        /// </returns>
        public abstract TPageObject CreateWebElement<TPageObject>(ISearchContext searchContext, By locator) where TPageObject:class;

        /// <summary>
        ///     Creates and initializes list of page elements and it's nested page objects.
        /// </summary>
        /// <typeparam name="TPageObject">
        ///     The type of the page object.
        /// </typeparam>
        /// <param name="searchContext">
        ///     The search context used for finding underlying WebElements.
        /// </param>
        /// <param name="locator">
        ///     The locator used for finding underlying WebElements.
        /// </param>
        /// <returns>
        ///     List of initialized page objects wrapping elements found in <paramref name="searchContext"/> with <paramref name="locator"/>.
        /// </returns>
        public abstract IList<TPageObject> CreateWebElementList<TPageObject>(ISearchContext searchContext, By locator);

        /// <summary>
        ///     Creates value assigned to page object member (field or property).
        /// </summary>
        /// <param name="memberType">Declared type of property or field</param>
        /// <param name="memberInfo">Field or property meta information</param>
        /// <param name="searchContext">Parent page object context</param>
        /// <returns>Initialized field or property value or null</returns>
        protected abstract object CreateMemberInstance(Type memberType, MemberInfo memberInfo, ISearchContext searchContext);

        /// <summary>
        ///     Create instance of page object class. It is not responsible for initializing page object members.
        /// </summary>
        /// <param name="pageObjectType">Type of page object being initialized</param>
        /// <param name="searchContext">
        ///     Optional constructor argument representing search context being wrapped.
        ///     It could be <see cref="IWebElement" /> or <see cref="IWebDriver" /> instance or other page object.
        /// </param>
        /// <returns>New instance of given type</returns>
        protected abstract object CreatePageObjectInstance(Type pageObjectType, ISearchContext searchContext);
    }
}
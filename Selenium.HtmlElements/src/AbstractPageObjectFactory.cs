using System;
using System.Reflection;
using HtmlElements.Elements;
using HtmlElements.Extensions;
using OpenQA.Selenium;

namespace HtmlElements
{
    /// <summary>
    ///     Implements <see cref="IPageObjectFactory"/> contract and delegates responsibility for creating actual page object and initializing it's members to subclasses.
    ///     This class takes care of locating suitable for initialization members and assigning value to it. 
    ///     It looks for all instance fields and properties which are web elements or web element lists and have not been assigned value yet.
    ///     Both fields and properties can be private protected or public, but in order to be initialized 
    ///     fields should not be marked <value>readonly</value> and properties should have setters.
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
        /// <param name="pageObjecType">Page object type</param>
        /// <param name="searchContext">Context used for finding elements</param>
        /// <returns>Fully initialized page object</returns>
        public object Create(Type pageObjecType, ISearchContext searchContext)
        {
            var pageObject = CreatePageObjectInstance(pageObjecType, searchContext);

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
                throw new ArgumentNullException("pageObject");
            }

            if (searchContext == null)
            {
                throw new ArgumentNullException("searchContext");
            }

            var pageObjectType = pageObject.GetType();

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
        public void Init(SearchContextWrapper pageObject)
        {
            Init(pageObject, pageObject);
        }

        /// <summary>
        ///     Creates value assigned to page object member (field or property).
        /// </summary>
        /// <param name="memberType">Declared type of property or field</param>
        /// <param name="memberInfo">Field or property meta information</param>
        /// <param name="searchContext">Parent page object context</param>
        /// <returns>Initialized field or property value or null</returns>
        protected abstract Object CreateMemberInstance(Type memberType, MemberInfo memberInfo, ISearchContext searchContext);

        /// <summary>
        ///     Create instance of page object class. It is not responsible for initializing page object members.
        /// </summary>
        /// <param name="pageObjectType">Type of page object being initialized</param>
        /// <param name="searchContext">
        ///     Optional constructor argument representing search context being wrapped. 
        ///     It could be <see cref="IWebElement"/> or <see cref="IWebDriver"/> instance or other page object.
        /// </param>
        /// <returns>New instance of given type</returns>
        protected abstract Object CreatePageObjectInstance(Type pageObjectType, ISearchContext searchContext);
    }
}
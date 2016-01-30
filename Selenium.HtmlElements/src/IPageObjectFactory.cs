using System;
using OpenQA.Selenium;

namespace HtmlElements
{
    /// <summary>
    ///     Creates and recursively initializes page object instances
    /// </summary>
    public interface IPageObjectFactory
    {
        /// <summary>
        ///     Creates and initializes page object of a given type and all nested page objects
        /// </summary>
        /// <typeparam name="TPageObject">Page object class</typeparam>
        /// <param name="searchContext">Context used for finding elements</param>
        /// <returns>Fully initialized page object</returns>
        TPageObject Create<TPageObject>(ISearchContext searchContext);

        /// <summary>
        ///     Creates and initializes page object of a given type and all nested page objects
        /// </summary>
        /// <param name="pageObjecType">Page object class</param>
        /// <param name="searchContext">Context used for finding elements</param>
        /// <returns>Fully initialized page object</returns>
        Object Create(Type pageObjecType, ISearchContext searchContext);

        /// <summary>
        ///     Initialize all nested page objects in given instance
        /// </summary>
        /// <param name="pageObject">Page object instance to be initialized</param>
        /// <param name="searchContext">Context used for finding elements</param>
        void Init(Object pageObject, ISearchContext searchContext);

        /// <summary>
        ///     Initialize page object wrapped WebDriver instance
        /// </summary>
        /// <param name="pageObject">Page object instance to be initialized</param>
        void Init(WebDriverWrapper pageObject);
    }
}
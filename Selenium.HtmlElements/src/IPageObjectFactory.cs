using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace HtmlElements
{
    /// <summary>
    ///     Creates and recursively initializes page object instances.
    /// </summary>
    public interface IPageObjectFactory
    {
        /// <summary>
        ///     Creates WebElement found with provided locator in given search context.
        /// </summary>
        /// <param name="searchContext">
        ///     Context used for finding element.
        /// </param>
        /// <param name="locator">
        ///     Element locator to use for finding element.
        /// </param>
        /// <returns>
        ///     Custom WebElement found in given search context with provided locator.
        /// </returns>
        IWebElement CreateWebElement(ISearchContext searchContext, By locator);

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
        TPageObject CreateWebElement<TPageObject>(ISearchContext searchContext, By locator) where TPageObject : class;

        /// <summary>
        ///     Creates list of WebElements found with provided locator in given search context.
        /// </summary>
        /// <param name="searchContext">
        ///     Context used for finding elements.
        /// </param>
        /// <param name="locator">
        ///     Element locator to use for finding elements.
        /// </param>
        /// <returns>
        ///     Custom list of WebElements.
        /// </returns>
        ReadOnlyCollection<IWebElement> CreateWebElementList(ISearchContext searchContext, By locator);

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
        IList<TPageObject> CreateWebElementList<TPageObject>(ISearchContext searchContext, By locator);

        /// <summary>
        ///     Creates and initializes page object of a given type and all nested page objects.
        /// </summary>
        /// <typeparam name="TPageObject">
        ///     Page object class.
        /// </typeparam>
        /// <param name="searchContext">
        ///     Context used for finding elements.
        /// </param>
        /// <returns>
        ///     Fully initialized page object.
        /// </returns>
        TPageObject Create<TPageObject>(ISearchContext searchContext);

        /// <summary>
        ///     Creates and initializes page object of a given type and all nested page objects.
        /// </summary>
        /// <param name="pageObjectType">
        ///     Page object class.
        /// </param>
        /// <param name="searchContext">
        ///     Context used for finding elements.
        /// </param>
        /// <returns>
        ///     Fully initialized page object.
        /// </returns>
        object Create(Type pageObjectType, ISearchContext searchContext);

        /// <summary>
        ///     Initialize all nested page objects in given instance.
        /// </summary>
        /// <param name="pageObject">
        ///     Page object instance to be initialized.
        /// </param>
        /// <param name="searchContext">
        ///     Context used for finding elements.
        /// </param>
        void Init(object pageObject, ISearchContext searchContext);

        /// <summary>
        ///     Initialize page object wrapped WebDriver instance.
        /// </summary>
        /// <param name="pageObject">
        ///     Page object instance to be initialized.
        /// </param>
        void Init(WebDriverWrapper pageObject);
    }
}
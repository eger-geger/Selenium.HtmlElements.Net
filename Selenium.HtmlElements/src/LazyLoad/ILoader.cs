using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    /// <summary>
    ///     Represents contract used for creating/caching objects lazily.
    /// </summary>
    /// <typeparam name="TObject">Type of object to be created/cached</typeparam>
    public interface ILoader<out TObject>
    {
        /// <summary>
        ///     Context used to load WebElement or WebElement list.
        /// </summary>
        ISearchContext SearchContext { get; }

        /// <summary>
        ///     Create new object instance or take it from cache
        /// </summary>
        /// <returns>Newly created or cached instance.</returns>
        TObject Load();

        /// <summary>
        ///     Reset cache if it exist. If cache is not supported then it should have no effect.
        /// </summary>
        void Reset();

        /// <summary>
        ///     Reset cache and create new instance. Similar to subsequent calls to <see cref="Reset"/> and <see cref="Load"/>
        /// </summary>
        /// <returns>Newly created or cached instance.</returns>
        TObject ResetAndLoad();
    }
}
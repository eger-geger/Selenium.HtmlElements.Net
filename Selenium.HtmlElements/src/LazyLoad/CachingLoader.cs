using System;
using OpenQA.Selenium;

namespace HtmlElements.LazyLoad
{
    internal abstract class CachingLoader<TObject> : ILoader<TObject> where TObject : class
    {
        private const int RetryCount = 10;

        private readonly bool _enableCache;

        private TObject _cached;

        protected CachingLoader(bool enableCache = true, TObject value = null)
        {
            _enableCache = enableCache;
            _cached = value;
        }

        public TObject Load()
        {
            if (!_enableCache)
            {
                Reset();
            }

            for (var i = 0; i < RetryCount && _cached == null; i++)
            {
                _cached = ExecuteLoad();
            }

            return _cached;
        }

        public virtual void Reset()
        {
            _cached = null;
        }

        public TObject ResetAndLoad()
        {
            Reset();

            return Load();
        }

        protected abstract TObject ExecuteLoad();

        public abstract ISearchContext SearchContext { get; }
    }
}
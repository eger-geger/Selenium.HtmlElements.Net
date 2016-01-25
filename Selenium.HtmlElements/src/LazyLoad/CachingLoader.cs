using System;

namespace HtmlElements.LazyLoad
{
    internal abstract class CachingLoader<TObject> : ILoader<TObject> where TObject : class
    {
        private readonly Boolean _enableCache;

        private TObject _cached;

        protected CachingLoader(Boolean enableCache = true, TObject value = null)
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

            while (_cached == null)
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
    }
}
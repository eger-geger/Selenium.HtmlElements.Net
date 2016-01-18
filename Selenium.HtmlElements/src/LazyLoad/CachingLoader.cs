namespace HtmlElements.LazyLoad
{
    internal abstract class CachingLoader<TObject> : ILoader<TObject> where TObject : class
    {
        private TObject _cached;

        protected CachingLoader(TObject initialValue = null)
        {
            _cached = initialValue;
        }

        public TObject Load()
        {
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
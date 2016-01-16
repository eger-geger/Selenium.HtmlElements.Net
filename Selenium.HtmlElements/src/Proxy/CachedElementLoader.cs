using System;
using System.Collections.Generic;
using System.Linq;

namespace HtmlElements.Proxy {

    internal abstract class CachedElementLoader<T> where T : class {

        private readonly Func<T> _doLoad;

        private readonly Predicate<T> _isLoaded;

        protected CachedElementLoader(Func<T> doLoad, Predicate<T> isLoaded) {
            _doLoad = doLoad;
            _isLoaded = isLoaded;
        }

        protected CachedElementLoader() {
            _doLoad = DoLoad;
            _isLoaded = IsLoaded;
        } 

        private T Loaded { get; set; }

        public IEnumerable<Type> IgnoredExceptionTypes { get; set; }

        public bool UseCash { get; protected set; }

        public T Load() {
            if (!UseCash) {
                Loaded = null;
            }

            while (!_isLoaded(Loaded)) {
                Loaded = LoadIgnoringExceptions();
            }

            return Loaded;
        }

        private T LoadIgnoringExceptions() {
            try {
                return _doLoad();
            } catch (Exception ex) {

                if (ShouldBeThrown(ex))
                {
                    throw;
                }

                return null;
            }     
        }

        private bool ShouldBeThrown(Exception ex) {
            return IgnoredExceptionTypes == null || !IgnoredExceptionTypes.Contains(ex.GetType());
        }

        protected virtual T DoLoad() {
            throw new NotImplementedException("should be overridden in subclass");
        }

        protected virtual bool IsLoaded(T target) {
            throw new NotImplementedException("should be overridden in subclass"); 
        }
    }

}
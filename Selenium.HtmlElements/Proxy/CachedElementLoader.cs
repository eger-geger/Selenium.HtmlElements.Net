using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

namespace HtmlElements.Proxy {

    internal abstract class CachedElementLoader<T> where T : class {

        protected readonly ILog Logger;

        private readonly Func<T> _doLoad;
        private readonly Predicate<T> _isLoaded;

        protected CachedElementLoader(Func<T> doLoad, Predicate<T> isLoaded) {
            _doLoad = doLoad;
            _isLoaded = isLoaded;

            Logger = LogManager.GetLogger(GetType());
        }

        protected CachedElementLoader() {
            _doLoad = DoLoad;
            _isLoaded = IsLoaded;

            Logger = LogManager.GetLogger(GetType());
        } 

        private T Cached { get; set; }

        public IEnumerable<Type> IgnoredExceptionTypes { get; set; }

        public T Load(bool useCached) {
            if (useCached && _isLoaded(Cached)) return Cached;

            while (!_isLoaded(Cached)) {
                Cached = LoadIgnoringExceptions();
            }

            return Cached;
        }

        private T LoadIgnoringExceptions() {
            try {
                return _doLoad();
            } catch (Exception ex) {
                
                if (ShouldBeThrown(ex)) throw;

                Logger.WarnFormat("Ignored: {0}", ex);

                return null;
            }     
        }

        private bool ShouldBeThrown(Exception ex) {
            return IgnoredExceptionTypes == null || !IgnoredExceptionTypes.Contains(ex.GetType());
        }

        protected virtual T DoLoad() {
            throw new NotImplementedException("Should Be Overridden in SubClass");
        }

        protected virtual bool IsLoaded(T target) {
            throw new NotImplementedException("Should Be Overridden in SubClass"); 
        }
    }

}
using System;
using System.Collections.Generic;

namespace HtmlElements.Actions {

    public class ConditionalActionExecutor<T> {

        private readonly T _target;

        private readonly Action<T> _action;
        
        private readonly List<Type> _ignoredExceptions = new List<Type>();

        private TimeSpan _timeout, _polling;

        public ConditionalActionExecutor(Action<T> action, T target) {
            _action = action;
            _target = target;
        }

        public ConditionalActionExecutor<T> For(TimeSpan timeout) {
            _timeout = timeout;

            return this;
        }

        public ConditionalActionExecutor<T> Every(TimeSpan polling) {
            _polling = polling;

            return this;
        }

        public ConditionalActionExecutor<T> Ignoring(params Type[] exceptions) {
            _ignoredExceptions.AddRange(exceptions);

            return this;
        }

        public void Until(Predicate<T> condition) {
            new ConditionalAction<T>(_action, condition) {
                Timeout = _timeout,
                PollingInterval = _polling,
                IgnoredExceptions = _ignoredExceptions
            }.Invoke(_target);
        }

    }

}
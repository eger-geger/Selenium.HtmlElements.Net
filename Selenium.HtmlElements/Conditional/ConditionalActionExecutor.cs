using System;
using System.Collections.Generic;

namespace Selenium.HtmlElements.Conditional {

    public class ConditionalActionExecutor<T> {

        private static readonly TimeSpan DefultTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan DefaultPolling = TimeSpan.FromMilliseconds(500);

        private readonly Action<T> _action;
        private readonly List<Type> _ignoredExceptions = new List<Type>();
        private TimeSpan _pollingInterval = DefaultPolling;

        private T _target;

        private TimeSpan _timeout = DefultTimeout;

        public ConditionalActionExecutor(Action<T> action) {
            _action = action;
        }

        public ConditionalActionExecutor<T> For(TimeSpan timeout) {
            _timeout = timeout;

            return this;
        }

        public ConditionalActionExecutor<T> Every(TimeSpan pollingInterval) {
            _pollingInterval = pollingInterval;

            return this;
        }

        public ConditionalActionExecutor<T> Ignoring(params Type[] exceptions) {
            _ignoredExceptions.AddRange(exceptions);

            return this;
        }

        public ConditionalActionExecutor<T> On(T target) {
            _target = target;

            return this;
        }

        public void Until(Predicate<T> condition) {
            new ConditionalAction<T>(_action, condition) {
                Timeout = _timeout,
                PollingInterval = _pollingInterval,
                IgnoredExceptions = _ignoredExceptions
            }.Invoke(_target);
        }
    }

}
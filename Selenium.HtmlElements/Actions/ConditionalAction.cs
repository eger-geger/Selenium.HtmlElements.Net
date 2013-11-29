using System;
using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium.Support.UI;

namespace Selenium.HtmlElements.Actions {

    public class ConditionalAction<T> {

        private readonly Action<T> _action;

        private readonly Predicate<T> _condition;

        public ConditionalAction(Action<T> action, Predicate<T> condition) {
            _action = action;
            _condition = condition;
        }

        public IEnumerable<Type> IgnoredExceptions { get; set; }

        public TimeSpan PollingInterval { get; set; }

        public TimeSpan Timeout { get; set; }

        public void Invoke(T target) {
            var wait = new DefaultWait<T>(target) {
                PollingInterval = PollingInterval,
                Timeout = Timeout,
                Message = string.Format("Failed to perform {0}", _action)
            };

            wait.IgnoreExceptionTypes(IgnoredExceptions.ToArray());
            wait.Until(InvokeConditionalAction);
        }

        private bool InvokeConditionalAction(T target) {
            if (!_condition(target)) _action(target);

            return _condition(target);
        }

    }

}
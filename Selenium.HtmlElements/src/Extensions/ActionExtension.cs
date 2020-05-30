using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace HtmlElements.Extensions {

    /// <summary>
    ///     Provides set of generic methods waiting for a specific condition
    /// </summary>
    public static class ActionExtension {

        private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(1);

        private static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        ///     Creates new instance of command builder setting current object as target, 
        ///     command timeout to 10 seconds and polling interval to 1 second
        /// </summary>
        /// <typeparam name="TTarget">Command target type</typeparam>
        /// <param name="target">Current object being command target</param>
        /// <param name="command">Command to execute on target</param>
        /// <returns>Command builder</returns>
        public static CommandBuilder<TTarget> Do<TTarget>(this TTarget target, Action<TTarget> command) where TTarget : class {
            return new CommandBuilder<TTarget>()
                .Execute(command)
                .WithTarget(target)
                .WithTimeout(DefaultPollingInterval)
                .WithInterval(DefaultCommandTimeout);
        }

        /// <summary>
        ///     Creates new instance of command builder setting current object as target, 
        ///     command timeout to 10 seconds and polling interval to 1 second
        /// </summary>
        /// <typeparam name="TTarget">Command target type</typeparam>
        /// <param name="target">Current object being command target</param>
        /// <param name="command">Command which does not require target</param>
        /// <returns>Command builder</returns>
        public static CommandBuilder<TTarget> Do<TTarget>(this TTarget target, Action command) where TTarget : class {
            return new CommandBuilder<TTarget>()
                .Execute(command)
                .WithTarget(target)
                .WithTimeout(DefaultCommandTimeout)
                .WithInterval(DefaultPollingInterval);
        }

        /// <summary>
        ///     Wait until command evaluates to not-null value or throw exception other then <see cref="WebDriverException"/>.
        ///     Throw <see cref="WebDriverTimeoutException"/> if command expires eventually. 
        ///     Current method overload evaluates command every 1 second and times out after 10 seconds.
        /// </summary>
        /// <typeparam name="TTarget">Type of the command target</typeparam>
        /// <typeparam name="TReturn">Type of the command return result</typeparam>
        /// <param name="target">Command target</param>
        /// <param name="command">Command to evaluate</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Command result if it succeeded otherwise exception will thrown</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown when command get timed out</exception>
        public static TReturn WaitFor<TTarget, TReturn>(this TTarget target, Func<TTarget, TReturn> command, string message = null) where TTarget : class {
            return WaitFor(target, command, DefaultCommandTimeout, message);
        }

        /// <summary>
        ///     Wait until command evaluates to not-null value or throw exception other then <see cref="WebDriverException"/>.
        ///     Throw <see cref="WebDriverTimeoutException"/> if command expires eventually. 
        ///     Current method overload evaluates command every 1 second.
        /// </summary>
        /// <typeparam name="TTarget">Type of the command target</typeparam>
        /// <typeparam name="TReturn">Type of the command return result</typeparam>
        /// <param name="target">Command target</param>
        /// <param name="command">Command to evaluate</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Command result if it succeeded otherwise exception will thrown</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown when command get timed out</exception>
        public static TReturn WaitFor<TTarget, TReturn>(
            this TTarget target, 
            Func<TTarget, TReturn> command, 
            TimeSpan commandTimeout, 
            string message = null) where TTarget : class {
            return WaitFor(target, command, commandTimeout, DefaultPollingInterval, message);
        }

        /// <summary>
        ///     Wait until command evaluates to not-null value or throw exception other then <see cref="WebDriverException"/>.
        ///     Throw <see cref="WebDriverTimeoutException"/> if command expires eventually.
        /// </summary>
        /// <typeparam name="TTarget">Type of the command target</typeparam>
        /// <typeparam name="TReturn">Type of the command return result</typeparam>
        /// <param name="target">Command target</param>
        /// <param name="command">Command to evaluate</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="pollingInterval">Determines how often command will be evaluated until it expires or succeeds</param>
        /// <param name="message">Error message used when command expires</param>
        /// <returns>Command result if it succeeded otherwise exception will thrown</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown when command get timed out</exception>
        public static TReturn WaitFor<TTarget, TReturn>(
            this TTarget target, 
            Func<TTarget, TReturn> command, 
            TimeSpan commandTimeout, 
            TimeSpan pollingInterval, 
            string message = null) where TTarget : class {
            var wait = new DefaultWait<TTarget>(target) {
                Message = message ?? string.Format("{0} expired after {1}", command, commandTimeout),
                PollingInterval = pollingInterval,
                Timeout = commandTimeout
            };

            wait.IgnoreExceptionTypes(typeof(WebDriverException));

            return wait.Until(command);
        }

        /// <summary>
        ///     Wait until condition evaluates to true or exception other then <see cref="WebDriverException"/> is thrown.
        ///     Throw <see cref="WebDriverTimeoutException"/> if command expires eventually.
        ///     Current method overload evaluates condition every 1 second and times out after 10 seconds.
        /// </summary>
        /// <typeparam name="TTarget">Type of the condition target</typeparam>
        /// <param name="target">Command target</param>
        /// <param name="condition">Condition to evaluate against target</param>
        /// <param name="message">Error message used when command expires</param>
        /// <exception cref="WebDriverTimeoutException">Thrown when condition times out</exception>
        public static void WaitUntil<TTarget>(this TTarget target, Predicate<TTarget> condition, string message = null) where TTarget : class {
            WaitUntil(target, condition, DefaultCommandTimeout, message);
        }

        /// <summary>
        ///     Wait until condition evaluates to true or exception other then <see cref="WebDriverException"/> is thrown.
        ///     Throw <see cref="WebDriverTimeoutException"/> if command expires eventually.
        ///     Current method overload evaluates condition every 1 second.
        /// </summary>
        /// <typeparam name="TTarget">Type of the condition target</typeparam>
        /// <param name="target">Command target</param>
        /// <param name="condition">Condition to evaluate against target</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="message">Error message used when command expires</param>
        /// <exception cref="WebDriverTimeoutException">Thrown when condition times out</exception>
        public static void WaitUntil<TTarget>(this TTarget target, Predicate<TTarget> condition, TimeSpan commandTimeout, string message = null) where TTarget : class {
            WaitUntil(target, condition, commandTimeout, DefaultPollingInterval, message);
        }

        /// <summary>
        ///     Wait until condition evaluates to true or exception other then <see cref="WebDriverException"/> is thrown.
        ///     Throw <see cref="WebDriverTimeoutException"/> if command expires eventually.
        /// </summary>
        /// <typeparam name="TTarget">Type of the condition target</typeparam>
        /// <param name="target">Command target</param>
        /// <param name="condition">Condition to evaluate against target</param>
        /// <param name="commandTimeout">Timeout after which command will became expired and exception will be thrown</param>
        /// <param name="pollingInterval">Determines how often command will be evaluated until it expires or succeeds</param>
        /// <param name="message">Error message used when command expires</param>
        /// <exception cref="WebDriverTimeoutException">Thrown when condition times out</exception>
        public static void WaitUntil<TTarget>(this TTarget target, Predicate<TTarget> condition, TimeSpan commandTimeout, TimeSpan pollingInterval, string message = null) where TTarget : class {
            WaitFor(target, condition.Invoke, commandTimeout, pollingInterval, message);
        }

    }

}
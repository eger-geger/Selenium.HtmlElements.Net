using System;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;

namespace HtmlElements.Extensions
{
    /// <summary>
    ///     Crates repeatable command being evaluated until command condition became true.
    /// </summary>
    /// <typeparam name="TTarget">Command target type</typeparam>
    public class CommandBuilder<TTarget>
    {
        private readonly List<Type> _ignoredExceptionTypes = new List<Type>();
        private readonly DefaultWait<CommandBuilder<TTarget>> _wait;
        private Action<TTarget> _command;
        private TTarget _target;

        /// <summary>
        ///     Create new instance of command builder
        /// </summary>
        public CommandBuilder()
        {
            _wait = new DefaultWait<CommandBuilder<TTarget>>(this);
        }

        /// <summary>
        ///     Set command to be executed on target
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> Execute(Action<TTarget> command)
        {
            _command = command;

            return this;
        }

        /// <summary>
        ///     Set command which does not require a target
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> Execute(Action command)
        {
            return Execute(target => command());
        }

        /// <summary>
        ///     Set command target. Synonym to <see cref="WithTarget" />
        /// </summary>
        /// <param name="target">Command target</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> On(TTarget target)
        {
            return WithTarget(target);
        }

        /// <summary>
        ///     Set command timeout. Synonym to <see cref="WithTimeout"/>
        /// </summary>
        /// <param name="commandTimeout">Command expiration timeout</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> For(TimeSpan commandTimeout)
        {
            return WithTimeout(commandTimeout);
        }

        /// <summary>
        ///     Set command polling interval. Synonym to <see cref="WithInterval"/>
        /// </summary>
        /// <param name="pollingInterval">Delay between sequential command executions</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> Every(TimeSpan pollingInterval)
        {
            return WithInterval(pollingInterval);
        }

        /// <summary>
        ///     Set message used to create exception when command times out
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> WithMessage(string errorMessage)
        {
            _wait.Message = errorMessage;

            return this;
        }

        /// <summary>
        ///     Set command target
        /// </summary>
        /// <param name="target">Command target</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> WithTarget(TTarget target)
        {
            _target = target;

            return this;
        }

        /// <summary>
        ///     Set command polling interval
        /// </summary>
        /// <param name="pollingInterval">Delay between sequential command executions</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> WithInterval(TimeSpan pollingInterval)
        {
            _wait.PollingInterval = pollingInterval;

            return this;
        }

        /// <summary>
        ///     Set command timeout
        /// </summary>
        /// <param name="commandTimeout">Command expiration timeout</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> WithTimeout(TimeSpan commandTimeout)
        {
            _wait.Timeout = commandTimeout;

            return this;
        }

        /// <summary>
        ///     Set types of exception which should be ignored when thrown by a command
        /// </summary>
        /// <param name="exceptions">Exception types to ignore</param>
        /// <returns>Current builder instance</returns>
        public CommandBuilder<TTarget> Ignoring(params Type[] exceptions)
        {
            _ignoredExceptionTypes.AddRange(exceptions);

            return this;
        }

        /// <summary>
        ///     Start execution loop in which command will be evaluated until condition evaluates to true 
        ///     or command timeout expires or command throw unexpected (not from ignore list) exception
        /// </summary>
        /// <param name="condition">Predicate telling weather command should be evaluated again</param>
        public void Until(Predicate<TTarget> condition)
        {
            _wait.IgnoreExceptionTypes(_ignoredExceptionTypes.ToArray());
            _wait.Until(CreateCommandExecutor(condition));
        }

        /// <summary>
        ///     Start execution loop in which command will be evaluated until condition evaluates to true 
        ///     or command timeout expires or command throw unexpected (not from ignore list) exception
        /// </summary>
        /// <param name="condition">Predicate telling weather command should be evaluated again</param>
        public void Until(Func<bool> condition)
        {
            Until(target => condition());
        }

        private Func<CommandBuilder<TTarget>, bool> CreateCommandExecutor(Predicate<TTarget> condition)
        {
            return builder =>
            {
                if (condition(_target))
                {
                    return true;
                }

                _command(_target);

                return condition(_target);
            };
        }
    }
}
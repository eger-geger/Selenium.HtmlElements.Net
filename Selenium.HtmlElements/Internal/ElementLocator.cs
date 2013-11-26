using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using OpenQA.Selenium;

namespace Selenium.HtmlElements.Internal {

    /// <summary>
    ///     Locates element all sets of elements concurrently using provided search context and list of by's
    /// </summary>
    internal class ElementLocator : IElementLocator {

        private readonly IEnumerable<By> _bys;
        private readonly ISearchContext _context;

        public ElementLocator(ISearchContext context, By by) : this(context, new List<By> {by}) {}

        public ElementLocator(ISearchContext context, IEnumerable<By> bys) {
            _context = context;
            _bys = bys;
        }

        public IWebElement FindElement() {
            var cancelTrigger = new CancellationTokenSource();

            var tasks =
                _bys.Select(@by => Task.Factory.StartNew(() => _context.FindElement(@by), cancelTrigger.Token)).ToList();

            var innerExceptions = new List<Exception>();

            while (tasks.Any()) {
                var finished = tasks[Task.WaitAny(tasks.ToArray())];

                tasks.Remove(finished);

                if (finished.IsFaulted && finished.Exception != null) innerExceptions.AddRange(finished.Exception.InnerExceptions);

                if (finished.Status == TaskStatus.RanToCompletion) {
                    cancelTrigger.Cancel();

                    return finished.Result;
                }
            }

            throw new NoSuchElementException(string.Format("Failed to locate element using: {0}", this),
                new AggregateException(innerExceptions));
        }

        public ReadOnlyCollection<IWebElement> FindElements() {
            var tasks = _bys.Select(@by => Task.Factory.StartNew(() => _context.FindElements(@by))).ToArray();

            Task.WaitAll(tasks);

            return tasks.SelectMany(t => t.Result).ToList().AsReadOnly();
        }

        public override string ToString() {
            var locatorsString = "[" + string.Join(",", _bys.Select(@by => by.ToString())) + "]";

            return string.Format("Bys: {0}, Context: {1}", locatorsString, _context);
        }

    }

}
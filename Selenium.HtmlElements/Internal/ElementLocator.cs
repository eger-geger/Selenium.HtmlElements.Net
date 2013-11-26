using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
            foreach (var @by in _bys) {
                try {
                    return _context.FindElement(@by);
                } catch (WebDriverException) {}
            }

            throw new NoSuchElementException(string.Format("Failed to locate element using: {0}", this));
        }

        public ReadOnlyCollection<IWebElement> FindElements() {
            var elements = new List<IWebElement>();

            foreach (var @by in _bys) {
                elements.AddRange(_context.FindElements(@by));
            }

            if (elements.Count == 0) throw new NoSuchElementException(string.Format("Failed to locate element using: {0}", this));

            return elements.AsReadOnly();
        }

        public override string ToString() {
            var locatorsString = "[" + string.Join(",", _bys.Select(@by => by.ToString())) + "]";

            return string.Format("Bys: {0}, Context: {1}", locatorsString, _context);
        }

    }

}
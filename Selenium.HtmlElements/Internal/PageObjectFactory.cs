using System;

using OpenQA.Selenium;

using log4net;

namespace Selenium.HtmlElements.Internal {

    internal static class PageObjectFactory {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(PageObjectFactory));

        public static object Create(Type type, ISearchContext context) {
            var emptyCtor = type.GetConstructor(new Type[0]);

            try {
                return emptyCtor != null ? emptyCtor.Invoke(new Object[0]) : Activator.CreateInstance(type, context);
            } catch (Exception ex) {
                Logger.FatalFormat("Failed to create {0} due {1}", type, ex.Message);

                throw;
            }
    
        }

    }

}
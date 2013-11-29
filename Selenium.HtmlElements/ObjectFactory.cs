using System;

namespace Selenium.HtmlElements {

    internal static class ObjectFactory {

        public static object Create(Type type, params object[] args) {
            var emptyCtor = type.GetConstructor(new Type[0]);

            return emptyCtor != null
                ? emptyCtor.Invoke(new object[0])
                : Activator.CreateInstance(type, args);
        }

    }

}
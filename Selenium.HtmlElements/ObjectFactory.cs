using System;
using OpenQA.Selenium;

namespace HtmlElements {

    public static class ObjectFactory
    {
        public delegate object PageObjectFactoryMethod(Type objectType, ISearchContext context);

        public static PageObjectFactoryMethod FactoryMethod { get; set; }

        public static object CreatePageObject(Type objecType, ISearchContext searchContext)
        {
            if (FactoryMethod != null)
            {
                return FactoryMethod.Invoke(objecType, searchContext);
            }

            var emptyConstructor = objecType.GetConstructor(new Type[0]);

            if (emptyConstructor != null)
            {
                return emptyConstructor.Invoke(new object[0]);
            }

            return Activator.CreateInstance(objecType, searchContext);
        }

    }

}
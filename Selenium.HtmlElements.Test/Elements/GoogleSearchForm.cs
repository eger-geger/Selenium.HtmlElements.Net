using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OpenQA.Selenium;

namespace Selenium2WebDriverSEd.Test.Elements
{
    public class GoogleSearchForm
    {
        public GoogleSearchForm(IWebElement wrapped) {
            Wrapped = wrapped;
        }

        public IWebElement Wrapped { get; private set; }

    }
}
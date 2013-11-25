using OpenQA.Selenium;

using Selenium.HtmlElements.Factory;

namespace Selenium.HtmlElements.Elements {

    public abstract class CustomElement : SearchContextWrapper {

        protected CustomElement(ISearchContext wrapped) : base(wrapped) {
            PageObjectActivator.Activate(this, wrapped);
        }

    }

}
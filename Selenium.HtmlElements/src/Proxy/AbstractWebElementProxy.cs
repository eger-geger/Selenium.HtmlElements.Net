using System;
using System.Collections.ObjectModel;
using System.Drawing;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;

namespace HtmlElements.Proxy
{
    internal abstract class AbstractWebElementProxy : IWebElement, IWrapsElement
    {
        protected readonly ILoader<IWebElement> Loader;

        protected AbstractWebElementProxy(ILoader<IWebElement> loader)
        {
            Loader = loader;
        }

        public ISearchContext GetShadowRoot()
        {
            return Execute(e => e.GetShadowRoot());
        }

        public string TagName
        {
            get { return Execute(e => e.TagName); }
        }

        public string Text
        {
            get { return Execute(e => e.Text); }
        }

        public bool Enabled
        {
            get { return Execute(e => e.Enabled); }
        }

        public bool Selected
        {
            get { return Execute(e => e.Selected); }
        }

        public Point Location
        {
            get { return Execute(e => e.Location); }
        }

        public Size Size
        {
            get { return Execute(e => e.Size); }
        }

        public bool Displayed
        {
            get { return Execute(e => e.Displayed); }
        }

        public virtual IWebElement FindElement(By @by)
        {
            return Execute(e => e.FindElement(@by));
        }

        public virtual ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return Execute(e => e.FindElements(@by));
        }

        public void Clear()
        {
            Execute(e => e.Clear());
        }

        public void SendKeys(string text)
        {
            Execute(e => e.SendKeys(text));
        }

        public void Submit()
        {
            Execute(e => e.Submit());
        }

        public void Click()
        {
            Execute(e => e.Click());
        }

        public string GetAttribute(string attributeName)
        {
            return Execute(e => e.GetAttribute(attributeName));
        }

        public string GetDomAttribute(string attributeName)
        {
            return Execute(e => e.GetDomAttribute(attributeName));
        }

        [Obsolete("Use the GetDomProperty method instead.")]
        public string GetProperty(string propertyName)
        {
            return Execute(e => e.GetProperty(propertyName));
        }

        public string GetDomProperty(string propertyName)
        {
            return Execute(e => e.GetDomProperty(propertyName));
        }

        public string GetCssValue(string propertyName)
        {
            return Execute(e => e.GetCssValue(propertyName));
        }

        public IWebElement WrappedElement => Loader.Load();

        protected bool Equals(WebElementProxy other)
        {
            return Equals(Loader, other.Loader);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((WebElementProxy) (object) (WebElementProxy) obj);
        }

        public override int GetHashCode()
        {
            return (Loader != null ? Loader.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return string.Format("{0} wrapping web element loaded by [{1}]", GetType().Name, Loader);
        }

        protected abstract void Execute(Action<IWebElement> action);

        protected TResult Execute<TResult>(Func<IWebElement, TResult> action)
        {
            var returnValue = default(TResult);

            Execute(element => { returnValue = action(element); });

            return returnValue;
        }
    }
}
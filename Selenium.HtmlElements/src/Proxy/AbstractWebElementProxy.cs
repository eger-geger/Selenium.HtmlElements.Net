using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using HtmlElements.Extensions;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Proxy
{
    internal abstract class AbstractWebElementProxy : IWebElement, IWrapsElement
    {
        protected readonly ILoader<IWebElement> Loader;

        protected AbstractWebElementProxy(ILoader<IWebElement> loader)
        {
            Loader = loader;
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

        public IWebElement WrappedElement
        {
            get { return Loader.Load(); }
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

        public string GetCssValue(string propertyName)
        {
            return Execute(e => e.GetCssValue(propertyName));
        }

        protected bool Equals(WebElementProxy other)
        {
            return Equals(Loader, other.Loader);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WebElementProxy) (object) (WebElementProxy) obj);
        }

        public override int GetHashCode()
        {
            return (Loader != null ? Loader.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("{0} wrapping web element loaded by", GetType())
                .AppendLine()
                .AppendLine(Loader.ToString().ShiftLinesToRight(2, '.'))
                .ToString();
        }

        protected abstract void Execute(Action<IWebElement> action);

        protected TResult Execute<TResult>(Func<IWebElement, TResult> action)
        {
            TResult returnValue = default(TResult);

            Execute(element => { returnValue = action(element); });

            return returnValue;
        }
    }
}
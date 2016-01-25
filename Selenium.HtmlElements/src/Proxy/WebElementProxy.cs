using System;
using System.Collections.ObjectModel;
using System.Drawing;
using HtmlElements.LazyLoad;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace HtmlElements.Proxy
{
    internal class WebElementProxy : IWebElement, IWrapsElement
    {
        private readonly ILoader<IWebElement> _loader;

        public WebElementProxy(ILoader<IWebElement> loader)
        {
            _loader = loader;
        }

        private void Execute(Action<IWebElement> action)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    action(_loader.Load());
                }
                catch (StaleElementReferenceException)
                {
                    _loader.Reset();

                    if (i == 4)
                    {
                        throw;    
                    }

                    continue;
                }

                break;
            }
        }

        private TResult Execute<TResult>(Func<IWebElement, TResult> action)
        {
            TResult returnValue = default(TResult);

            Execute(element => { returnValue = action(element); });

            return returnValue;
        }

        public IWebElement FindElement(By @by)
        {
            return Execute(e => e.FindElement(@by));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
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
            get { return _loader.Load(); }
        }

        protected bool Equals(WebElementProxy other)
        {
            return Equals(_loader, other._loader);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WebElementProxy) obj);
        }

        public override int GetHashCode()
        {
            return (_loader != null ? _loader.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return String.Format("Proxy wrapping [{0}]", _loader);
        }
    }
}

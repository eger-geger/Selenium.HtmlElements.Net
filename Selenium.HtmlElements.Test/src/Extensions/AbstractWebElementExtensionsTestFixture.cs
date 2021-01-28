using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Extensions
{
    public abstract class AbstractWebElementExtensionsTestFixture
    {
        protected Mock<IWebElement> ElementMock;

        [SetUp]
        public void SetUpMocks()
        {
            ElementMock = new Mock<IWebElement>();
        }

        protected static void ExecuteAsync(Action action, TimeSpan delay)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delay);
                action();
            });
        }
    }
}
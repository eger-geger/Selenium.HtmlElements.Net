using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using HtmlElements.Elements;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using OpenQA.Selenium;
#pragma warning disable 649
#pragma warning disable 169

namespace HtmlElements.Test
{
    [Parallelizable(ParallelScope.Self)]
    public class AbstractPageObjectFactoryTests
    {
        private AbstractPageObjectFactory _factory;

        private Mock<AbstractPageObjectFactory> _factoryMock;

        private IWebDriver _searchContext;

        [SetUp]
        public void SetUpMocks()
        {
            _factoryMock = new Mock<AbstractPageObjectFactory>(MockBehavior.Loose);
            _searchContext = new Mock<IWebDriver>(MockBehavior.Loose).Object;
            _factory = _factoryMock.Object;

            _factoryMock.Protected()
                .Setup<object>("CreatePageObjectInstance", typeof(PageObjectA), _searchContext)
                .Returns(new PageObjectA(_searchContext));
        }
        
        [Test]
        public void ShouldRaiseArgumentErrorWhenInitializingNullObject()
        {
            Assert.That(() => _factory.Init(null,  _searchContext), Throws.ArgumentNullException);
        }
        
        [Test]
        public void ShouldRaiseArgumentErrorWhenInitializingWithNullContext()
        {
            Assert.That(() => _factory.Init(new PageObjectA(_searchContext),  null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void ShouldAskToCreatePageObjectInstanceWhenCalledCreateWithGenericArgument()
        {
            var pageObjectA = _factory.Create<PageObjectA>(_searchContext);

            VerifyPageObjectCreated(pageObjectA);
            VerifyPageObjectCreationRequestWasMade();
            VerifyMemberInitializationRequestsWereMade(_searchContext);
        }

        [Test]
        public void ShouldAskToCreatePageObjectInstanceWhenCalledCreateWithTypeArgument()
        {
            var pageObjectA = _factory.Create(typeof(PageObjectA), _searchContext);

            VerifyPageObjectCreated(pageObjectA as PageObjectA);
            VerifyPageObjectCreationRequestWasMade();
            VerifyMemberInitializationRequestsWereMade(_searchContext);
        }

        [Test]
        public void ShouldAskToInitializeEveryNestedPageObjectWhenInitializingPageObject()
        {
            var pageObjectA = new PageObjectA(_searchContext);

            _factory.Init(pageObjectA, _searchContext);

            VerifyPageObjectCreated(pageObjectA);
            VerifyMemberInitializationRequestsWereMade(_searchContext);
        }

        [Test]
        public void ShouldAskToInitializeEveryNestedPageObjectWhenInitializingContextWrapper()
        {
            var contextWrapper = new PageObjectA(_searchContext);

            _factory.Init(contextWrapper);

            VerifyMemberInitializationRequestsWereMade(contextWrapper);
        }

        private void VerifyPageObjectCreated(PageObjectA pageObject)
        {
            Assert.That(pageObject, Is.Not.Null);
            Assert.That(pageObject.PageObjectFactory, Is.Not.Null.And.SameAs(_factory));
        }

        private void VerifyPageObjectCreationRequestWasMade()
        {
            _factoryMock.Protected().Verify("CreatePageObjectInstance", Times.Once(),
                typeof(PageObjectA), _searchContext);
        }

        private void VerifyMemberInitializationRequestsWereMade(ISearchContext context)
        {
            _factoryMock.Protected().Verify<object>("CreateMemberInstance", Times.Once(),
                typeof(HtmlElement), ItIsMemberNamed("_elementA"), context);

            _factoryMock.Protected().Verify<object>("CreateMemberInstance", Times.Once(),
                typeof(IWebElement), ItIsMemberNamed("ElementB"), context);

            _factoryMock.Protected().Verify<object>("CreateMemberInstance", Times.Once(),
                typeof(IList<IWebElement>), ItIsMemberNamed("_elementListA"), context);

            _factoryMock.Protected().Verify<object>("CreateMemberInstance", Times.Once(),
                typeof(HtmlElement), ItIsMemberNamed("ElementC"), context ?? _searchContext);

            _factoryMock.Protected().Verify<object>("CreateMemberInstance", Times.Once(),
                typeof(IList<HtmlImage>), ItIsMemberNamed("ElementListB"), context ?? _searchContext);
        }

        private static Expression ItIsMemberNamed(string memberName)
        {
            return ItExpr.Is<MemberInfo>(member => member.Name == memberName);
        }

        private class PageObjectA : WebDriverWrapper
        {
            private HtmlElement _elementA;

            protected IWebElement ElementB;

            private IList<IWebElement> _elementListA;

            public PageObjectA(ISearchContext webElement) : base(webElement)
            {
            }

            public new IPageObjectFactory PageObjectFactory => base.PageObjectFactory;

            public HtmlElement ElementC { get; private set; }

            public IList<HtmlImage> ElementListB { get; private set; }
        }
    }
}
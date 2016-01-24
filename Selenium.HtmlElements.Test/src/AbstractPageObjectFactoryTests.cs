using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using HtmlElements.Elements;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test
{
    public class AbstractPageObjectFactoryTests
    {
        private AbstractPageObjectFactory _factory;
        private Mock<AbstractPageObjectFactory> _factoryMock;
        private ISearchContext _searchContext;

        [SetUp]
        public void SetUpMocks()
        {
            _factoryMock = new Mock<AbstractPageObjectFactory>(MockBehavior.Loose);
            _searchContext = new Mock<ISearchContext>(MockBehavior.Loose).Object;
            _factory = _factoryMock.Object;

            _factoryMock.Protected()
                .Setup<Object>("CreatePageObjectInstance", typeof(PageObjectA), _searchContext)
                .Returns(new PageObjectA(_searchContext));
        }

        [Test]
        public void ShouldAskToCreatePageObjectInstanceWhenCalledCreateWithGenericArgument()
        {
            _factory.Create<PageObjectA>(_searchContext);

            VerifyPageObjectCreationRequestWasMade();
            VerifyMemberInitializationRequestsWereMade(_searchContext);
        }

        [Test]
        public void ShouldAskToCreatePageObjectInstanceWhenCalledCreateWithTypeArgument()
        {
            _factory.Create(typeof (PageObjectA), _searchContext);

            VerifyPageObjectCreationRequestWasMade();
            VerifyMemberInitializationRequestsWereMade(_searchContext);
        }

        [Test]
        public void ShouldAskToInitializeEveryNestedPageObjectWhenInitializingPageObject()
        {
            _factory.Init(new PageObjectA(_searchContext), _searchContext);

            VerifyMemberInitializationRequestsWereMade(_searchContext);
        }

        [Test]
        public void ShouldAskToInitializeEveryNestedPageObjectWhenInitializingContextWrapper()
        {
            var contextWrapper = new PageObjectA(_searchContext);

            _factory.Init(contextWrapper);

            VerifyMemberInitializationRequestsWereMade(contextWrapper);
        }

        private void VerifyPageObjectCreationRequestWasMade()
        {
            _factoryMock.Protected().Verify("CreatePageObjectInstance", Times.Once(),
                typeof (PageObjectA), _searchContext);
        }

        private void VerifyMemberInitializationRequestsWereMade(ISearchContext context)
        {
            _factoryMock.Protected().Verify<Object>("CreateMemberInstance", Times.Once(),
                typeof(HtmlElement), ItIsMemberNamed("_elementA"), context);

            _factoryMock.Protected().Verify<Object>("CreateMemberInstance", Times.Once(),
                typeof(IWebElement), ItIsMemberNamed("_elementB"), context);

            _factoryMock.Protected().Verify<Object>("CreateMemberInstance", Times.Once(),
                typeof(IList<IWebElement>), ItIsMemberNamed("_elementListA"), context);

            _factoryMock.Protected().Verify<Object>("CreateMemberInstance", Times.Once(),
                typeof (HtmlElement), ItIsMemberNamed("ElementC"), context ?? _searchContext);

            _factoryMock.Protected().Verify<Object>("CreateMemberInstance", Times.Once(),
                typeof(IList<HtmlImage>), ItIsMemberNamed("ElementListB"), context ?? _searchContext);
        }

        private static Expression ItIsMemberNamed(String memberName)
        {
            return ItExpr.Is<MemberInfo>(member => member.Name == memberName);
        }

        private class PageObjectA : SearchContextWrapper
        {
            private HtmlElement _elementA;
            protected IWebElement _elementB;
            private IList<IWebElement> _elementListA;

            public PageObjectA(ISearchContext wrapped) : base(wrapped)
            {
            }

            public HtmlElement ElementC { get; private set; }
            public IList<HtmlImage> ElementListB { get; private set; }
        }
    }
}
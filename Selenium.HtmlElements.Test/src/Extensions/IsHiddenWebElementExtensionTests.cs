﻿using System.Drawing;
using HtmlElements.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace HtmlElements.Test.Extensions
{
    public class IsHiddenWebElementExtensionTests : AbstractWebElementExtensionsTestFixture
    {
        [Test]
        public void IsHiddenShouldYieldTrueWhenThereIsNoSuchElement()
        {
            ElementMock.Setup(e => e.Size).Throws<NoSuchElementException>();

            Assert.True(ElementMock.Object.IsHidden());
        }

        [Test]
        public void IsHiddenShouldReturnTrueWhenElementIsNotDisplayed()
        {
            ElementMock.Setup(e => e.Size).Returns(new Size(100, 100));
            ElementMock.Setup(e => e.Displayed).Returns(false);

            Assert.True(ElementMock.Object.IsHidden());
        }

        [Test]
        public void IsHiddenShoutReturnFalseWhenElementIsDisplayed()
        {
            ElementMock.Setup(e => e.Size).Returns(new Size(100, 100));
            ElementMock.Setup(e => e.Displayed).Returns(true);

            Assert.False(ElementMock.Object.IsHidden());
        }
    }
}

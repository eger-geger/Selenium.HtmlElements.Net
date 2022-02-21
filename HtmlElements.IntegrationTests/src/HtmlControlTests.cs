using HtmlElements.Extensions;
using NUnit.Framework;

namespace HtmlElements.IntegrationTests;

public class HtmlControlTests : IntegrationTestFixture
{
    [Test]
    public void ShouldGetSetHtmlControlValue()
    {
        Assert.That(PageAlpha.Input.Value, Is.EqualTo("42"));
        PageAlpha.Input.Value = "43";
        var value = PageAlpha.Input.ExecuteScriptOnSelf("return {self}.value");
        Assert.That(value, Is.EqualTo("43"));
    }
    
}
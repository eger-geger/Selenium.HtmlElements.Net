**HtmlElements** is .NET library complementing to [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) page object model by allowing you to create more complex and sophysticated page objects. It also provides set of standard page objects for commonly used HTML elements (links, input fields, images, frames, etc), alternative wait syntax, smart frames and some other usefull utilities. You can add it to your project by installing [HtmlElements](http://www.nuget.org/packages/HtmlElements/) nuget package. More information can be found in [API reference](http://eger-geger.github.io/Selenium.HtmlElements.Net/).

## Custom page objects ##

The main goal of **HtmlElements** library is to represent any given page or part of a page as set of smaller reusable components. Every component is a class having any number of nested components and public methods. Smaller components create a hierarchy similar to DOM with a page component at the top.

Assuming we would like to model a page listing nuget packages (https://www.nuget.org/packages) we could describe every package from list as a seprate component and then have list of such components inside page object descriding page as a whole.

```C#

public class NugetPackageListPage {
    
    [FindsBy(How = How.CssSelector, Using = "#searchBoxInput"), CacheLookup]
    private IWebElement _searchInput;

    [FindsBy(How = How.CssSelector, Using = "#searchBoxSubmit"), CacheLookup]
    private IWebElement _searchBtn;

    public IList<NugetPackageItem> Packages { get; private set; }

    public void Search(String query){
        _searchInput.EnterText(query);
        _searchBtn.Click();
    }

}

public class NugetPackageItem : HtmlElement {

    [FindsBy(How = How.CssSelector, Using = "h1>a"), CacheLookup]
    private HtmlElement _name;

    [FindsBy(How = How.CssSelector, Using = "article>p:nth-child(2)"), CacheLookup]
    private HtmlElement _description;

    [FindsBy(How = How.CssSelector, Using = ".downloads"), CacheLookup]
    private HtmlElement _downloads;

    public override By DefaultLocator => By.CssSelector("#searchResults li");
    
    public String Name { 
        get {
            return _name.Text;
        }
    }

    public String Description {
        get {
            return _description.Text;
        }
    }

    public String Downloads {
        get {
            return _downloads.Text;
        }
    }
}

IWebDriver webDriver = new FirefoxDriver();

IPageObjectFactory pageFactory = new PageObjectFactory();

NugetPackageListPage page = pageFactory.Create<NugetPackageListPage>(webDriver);

```

[PageObjectFactory](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_page_object_factory.html) is creating and initializing page objects. It is one of the core library components since it creates and recursively initializes all page objects in a given hierarchy. Page factory can initialize fields and properties which type is derived from _IWebElement_ or _IList<IWebElement>_. It can create (and initilize) page object of any type using default or custom constructor. It also provides few extension points.

Default page factory implementation wraps raw WebElements located in browser into proxy loading elements on demand and hanling _StaleElementReferenceExcetion_ by reloading the underlying element. It is possible to change how proxy is being created by implementing [IProxyFactory](http://eger-geger.github.io/Selenium.HtmlElements.Net/interface_html_elements_1_1_proxy_1_1_i_proxy_factory.html) interface and how elements are loaded by implementing [ILoaderFactory](http://eger-geger.github.io/Selenium.HtmlElements.Net/interface_html_elements_1_1_lazy_load_1_1_i_loader_factory.html) interface. Also it is possible to change the way how new page objects are being instantiated by overriding [CreatePageObjectInstance](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_abstract_page_object_factory.html#ae50af112bab9ed9f68b2df6c0be59553) default factory method and how page object members are being initialized by overriding [CreateMemberInstance](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_abstract_page_object_factory.html#aa7aecd99f56e12bdf1e4ee62d84575df) method.

Please refer to API reference for more details.

### Page element default locator

Some web components can be located using the same locators regardless a page they present on.

In this case you can override default locator (see `NugetPackageItem.DefaultLocator`) and omit [FindsBy] attribute rather then duplicate it across page object or page element classes.
If `DefaultLocator` property is not defined, element[s] will be located `By.Id` using page object property name as id value.

## Standart HTML elements ##

In addition to creating your custom page objects you can use set predefined components which models commonly used DOM elements. Following components can be found in [HtmlElements.Elements](http://eger-geger.github.io/Selenium.HtmlElements.Net/namespace_html_elements_1_1_elements.html) namespace:

* HtmlElement
* HtmlLink
* HtmlImage
* HtmlFrame
* HtmlInput
* HtmlCheckBox
* HtmlSelect
* HtmlTextArea
* HtmlTable

All of the above components are derived from [HtmlElement](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_elements_1_1_html_element.html) which is also a good candidate to derive custom components from.

## Alternative wait syntax ##

While writing selenium tests you'll often found your self waiting until something happens in browser. It could be slow loading page, dialog which takes few seconds to show up or hide, background proccess which must finish until test can proceed. The simple way is to stop thread for few seconds. Reliable way is to use _WebDriverWait_ or _DefaultWait_ classes being part of selenium webdriver library. The convinient way is to use extension methods provided by current library. Just take a look on few examples below.

```C#

/* assuming there is a highly dynamic login button which takes some time to load */

[FindsBy(How = How.CssSelector, Using = "#login")]
private IWebElement _loginButton;

/* wait for 10 seconds until button became displayed for and click on it */
_loginButton.WaitForPresent().Click();

/* wait for 10 seconds until button became displayed and click on it or throw exception with given message */
_loginButton.WaitForVisible("login button did not became visible");

/* wait for 30 seconds until button became hidden */
_loginButton.UntilHidden(TimeSpan.FromSeconds(30));

/* wait for 60 seconds until button text becames empty checking it every 5 seconds */
_loginButton.WaitUntil(btn => String.IsNullOrEmpty(btn.Text), TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(5));

/* click on button every 30 seconds up to five minutes until it becames hidden */
_loginButton
    .Do(btn => btn.click())
    .For(TimeSpan.FromSeconds(300))
    .Every(TimeSpan.FromSeconds(30))
    .Until(btn => !btn.IsHidden());

```

So basically you need to say what are you waiting for ? For how long ? How often to check weather it happened ? What message should timeout axception be thrown with ?

## Smart frames ##

Sometimes tests need to interact with HTML frames. In order to do it we need to switch wedriver context to specific frame firts and switch it back after we've done. It is exactly what [FrameContextOverride](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_frame_context_override.html) class is desgined to do.

```C#

/* assuming there is an frame defined as following */
[FindsBy(How = How.CssSelector, Using = "iframe")]
private IWebElement _frameElement;

/* and IWebDriver instance is defined as 'webDriver' */

/* the following will do stuff in it and switch back */
using (new FrameContextOverride(webDriver, _frameElement))
{
    _frameElement.FindElement(By.Id("email")).SendKeys("joe@gmail.com");
    _frameElement.FindElement(By.Id("password")).SendKeys("qwerty");
    _frameElement.FindElement(By.Id("signin")).Click();
}

```

[HtmlFrame](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_elements_1_1_html_frame.html) is another class which makes life easier when it comes to working with frames. When created by default page object factory (described above) it's wrapped search context is set to WebDriver instance instead of WebElement as it is done for other custom elements. It allows using it as a base class for custom frame page objects. Previous example could be rewritten as shown below.

```C#

public class SignInFrame : HtmlFrame {

    [FindsBy(How = How.CssSelector, Using = "#email")]
    private IWebElement _emailField;

    [FindsBy(How = How.CssSelector, Using = "#password")]
    private IWebElement _passwordField;

    [FindsBy(How = How.CssSelector, Using = "#signin")]
    private IWebElement _signinBtn;

    public SignInFrame (IWebElement webElement) : base (webElement) {}

    public void SignIn(String email, String password) 
    {
        using(new FrameContextOverride(this))
        {
            _emailField.SendKeys(email);
            _emailField.SendKeys(password);
            _signinBtn.Click();
        }
    }

}

public class HomePage {
    
    [FindsBy(How = How.CssSelector, Using = "iframe")]
    public SignInFrame { get; set; }

}

```

## Implicit wait override ##

There might be cases when you might want to change default implicit wait WebDriver option for specific operation and restore it back after it is done. For example you might set to 30 seconds for most cases but do not want to wait for element which you expect to be removed from DOM. You could use [ImplicitWaitOverride](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_implicit_wait_override.html) for it's purpose.

```C#

TimeSpan defaultImplicitWait = TimeSpan.FromSeconds(30);

TimeSpan overridenImplcitWait = TimeSpan.FromSeconds(2);

using(new ImplicitWaitOverride(webDriver, defaultImplicitWait, overridenImplcitWait)){
    loginDialog.WaitUntilHidden();
}

```

## Element groups ##

Often people writing selenium tests want to check weather some element or another exist on a page. There are two possible way of doing so: expose raw WebElements or create _IsXXXDisplayed_ properties. Author deslike both and the idea itself. Newertheless people still need to do it. Element group is attempt to make it in a way which does not break encapsulation. How it works:

1. mark your private WebElement fields and properties with [ElementGroupAttribute](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_element_group_attribute.html) providing one or multiple group names;
2. create [ElementGroup](http://eger-geger.github.io/Selenium.HtmlElements.Net/class_html_elements_1_1_element_group.html) object in your tests specifying the name of the group you are interested in;
3. use created group to retrieve list of WebElements with matching group name from page object.

```C#

public class LoginPage {

    [ElementGroup("login_form")]
    private IWebElement _loginButton;

    [ElementGroup("login_form")]
    private IWebElement _loginInput;

    [ElementGroup("login_form")]
    private IWebElement _passwordInput;
}

/* and later on it test */

var loginPage = new LoginPage();
var loginFormElementGroup = new ElementGroup("login_form");
IList<IWebElement> loginFormElements = loginFormElementGroup.GetElements(loginPage);

```
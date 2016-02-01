**HtmlElements** is .NET library extending [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) page object model by allowing you to create more complex and sophysticated page objects. It also provides set of standard page objects for commonly used HTML elements (links, input fields, images, frames, etc), alternative wait syntax, smart frames and some other usefull utilities. You can add it to your project by installing [HtmlElements](http://www.nuget.org/packages/HtmlElements/) nuget package.

## Custom page objects ##

The main goal of **HtmlElements** library is to represent any given page or part of a page as set of smaller reusable components. Every component is a class having any number of nested components and public methods. Page is serving as a root of component hierarchy.

Assuming we would like to model a page listing nuget packages (https://www.nuget.org/packages) we could describe every package from list as a seprate component and then have list of such components inside page object descriding page as a whole.

```C#

/* describe package list page */
public class NugetPackageListPage {
    
    [FindsBy(How = How.CssSelector, Using = "#searchBoxInput"), CacheLookup]
    private IWebElement _searchInput;

    [FindsBy(How = How.CssSelector, Using = "#searchBoxSubmit"), CacheLookup]
    private IWebElement _searchBtn;

    [FindsBy(How = How.CssSelector, Using = "#searchResults li")]
    public IList<NugetPackageItem> Packages { get; private set; }

    public void Search(String query){
        _searchInput.EnterText(query);
        _searchBtn.Click();
    }

}

/* describe package item component */
public class NugetPackageItem : HtmlElement {

    [FindsBy(How = How.CssSelector, Using = "h1>a"), CacheLookup]
    private HtmlElement _name;

    [FindsBy(How = How.CssSelector, Using = "article>p:nth-child(2)"), CacheLookup]
    private HtmlElement _description;

    [FindsBy(How = How.CssSelector, Using = ".downloads"), CacheLookup]
    private HtmlElement _downloads;

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

/* initilize WebDriver */
IWebDriver webDriver = new FirefoxDriver();

/* create default factory */
IPageObjectFactory pageFactory = new PageObjectFactory();

/* create page instance */
NugetPackageListPage page = pageFactory.Create<NugetPackageListPage>(webDriver);

```

Instance of _PageObjectFactory_ class is used to create and initialize page instance being a hierarchy root. It is one of the core library components since it creates and recursively initializes all page objects in a given hierarchy. Page factory can initialize fields and properties which type is derived from _IWebElement_ or _IList<IWebElement>_. It can create (and initilize) page object of any type using default or custom constructor. It also provides few extension points.

Default page factory implementation wraps raw web elements located in browser into proxy loading elements on demand and hanling _StaleElementReferenceExcetion_ by reloading the underlying element. It is possible to change how proxy is being created (by implementing IProxyFactory interface), how elements are being loaded (by implementing ILoaderFactory interface). Also you can change the way how new page objects are being instantiated (by overriding _CreatePageObjectInstance_ default factory method) and how page object members are being initialized (by overriding _CreateMemberInstance_ default factory method).

Please refer to API reference for more details.

## Standart HTML elements ##

In addition to creatign your custom page objects you can use set predefined componenets which models commonly used DOM elements. Following components can be found in _HtmlElements.Elements_ namespace:

* HtmlElement
* HtmlLink
* HtmlImage
* HtmlFrame
* HtmlInput
* HtmlCheckBox
* HtmlSelect
* HtmlTextArea

All of the above components are derived from _HtmlElement_ which also a good candidate to derive custom components from.

## Alternative wait syntax ##
While writing selenium tests you'll often found your self waiting until something happens in browser. It could be slow loading page, dialog which takes few seconds to show up or hide, background proccess which must finish until test can proceed. The simple way is to wait for few seconds. Reliable way is to use _WebDriverWait_ or _DefaultWait_ classes being part of selenium webdriver library. The convinient way is to use extension methods provided by current library. Just take a look on example below.

```C#

/* assuming there is a highly dynamic login button which takes some time to load */

[FindsBy(How = How.CssSelector, Using = "#login")]
private IWebElement _loginButton;

/* we can wait for weird stuff happenning with it */

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

Yeah, examples above are very realistic but they do show what etensions can do and how it can be used. You could check API reference for list of available extensions and complete signatures.

## Smart (kind of) frames ##
Sometimes tests need to interact with HTML frames. In order to do it we need to switch wedriver context to specific frame firts and (in most cases) switch it back after we've done. It is exactly what _FrameContextOverride_ class is desgined to do.

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

_HtmlFrame_ is nother class which makes life easier when it coming to working with frames. When created by default page object factory (described above) it's wrapped search context is set to WebDriver instance instead of WebElement (as it is done for other custom elements). It allows using it as a base class for custom page objects for describing complex frames objects. Example above could be rewritten as shown below.

```C#

/* creating frame page object */
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

/* and using it on a page */

public class HomePage {
    
    [FindsBy(How = How.CssSelector, Using = "iframe")]
    public SignInFrame { get; set; }

}

```

## Oher goodies ##


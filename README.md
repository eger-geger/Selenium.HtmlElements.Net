**HtmlElements** is .NET library extending [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) page object model by allowing you to create more complex and sophysticated page objects. It also provides set of standard page objects for commonly used HTML elements (links, input fields, images, frames, etc), alternative wait syntax, smart frames and some other usefull utilities. You can add it to your project by installing [HtmlElements](http://www.nuget.org/packages/HtmlElements/) nuget package.

## Custom page objects ##

The main idea behind HtmlElements library is to represent any given web page or part of a page as set of smaller reusable components. Every component is a  class having any number of nested components and public methods for itenracting with it. Page is serving as a root of component hierarchy.

Assuming we would like to create a page object for page listing nuget packages (https://www.nuget.org/packages) we could describe every package from list as a seprate components and then have list of such components on page oject descriding page as a whole.

```

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

As you can see from the code above instance of _PageObjectFactory_ class is used to create and initialize page instance being a hierarchy root. It is one of the core library components since it creates and recursively initializes all page objects in a given hierarchy. Page factory can initialize fields and properties which type is derived from _IWebElement_ or _IList<IWebElement>_. _PageFactory_ can create (and initilize) page object of any type using default or custom constructor. It also provides few extension points allowing you to change 

* how page objects get created; 
* how it's members get initialized; 
* how underlying web elements are located. 

Default page factory implementation wraps raw web elements located in browser into proxy loading elements on demand and hanling _StaleElementReferenceExcetion_ by reloading the underlying element.

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

All of the above components are derived from _HtmlElement_ which also candidate to derive from when creating custom components.

## Alternative wait syntax ##

## Smart (kind of) frames ##

## Oher goodies ##


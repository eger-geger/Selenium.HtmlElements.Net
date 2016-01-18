using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlElements.Elements
{
    interface IPageObjectFactoryWrapper
    {

        IPageObjectFactory PageObjectFactory { get; set; }

    }
}

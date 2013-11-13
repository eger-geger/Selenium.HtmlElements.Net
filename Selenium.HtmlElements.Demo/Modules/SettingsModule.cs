using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject.Modules;

namespace Selenium.HtmlElements.Demo.Modules
{

    internal class SettingsModule : NinjectModule {

        public override void Load() {
            Bind<string>().ToConstant("justanswer.com").Named("AppHost");
        }

    }

}

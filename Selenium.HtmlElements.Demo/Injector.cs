using System;

using Ninject;

using Selenium.HtmlElements.Demo.Modules;

namespace Selenium.HtmlElements.Demo {

    internal static class Injector {

        public static readonly IKernel Kernel;

        static Injector() {
            Kernel = new StandardKernel(new SettingsModule());
        }

    }

}
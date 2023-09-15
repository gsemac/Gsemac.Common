using Gsemac.Reflection.Plugins;
using System;

namespace Gsemac.Net.WebDrivers {

    public sealed class RequiresSeleniumAttribute :
        RequirementAttributeBase {

        // Public members

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            return new RequiresAssemblyOrTypesAttribute("WebDriver", "OpenQA.Selenium.IWebDriver")
                .TestRequirement(serviceProvider);

        }

    }

}
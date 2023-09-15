using Gsemac.Reflection.Plugins;

namespace Gsemac.Net.WebDrivers {

    public sealed class RequiresSeleniumAttribute :
        RequirementAttributeBase {

        // Public members

        public RequiresSeleniumAttribute() :
            base(new RequiresAssemblyOrTypesAttribute("WebDriver", "OpenQA.Selenium.IWebDriver")) {
        }

    }

}
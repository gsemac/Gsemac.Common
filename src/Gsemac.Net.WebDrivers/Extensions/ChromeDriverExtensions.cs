using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace Gsemac.Net.WebDrivers.Extensions {

    public static class ChromeDriverExtensions {

        public static void AddScriptToEvaluateOnNewDocument(this ChromeDriver webDriver, string script) {

            var parameters = new Dictionary<string, object> {
                ["source"] = script
            };

            webDriver.ExecuteChromeCommand("Page.addScriptToEvaluateOnNewDocument", parameters);

        }

    }

}
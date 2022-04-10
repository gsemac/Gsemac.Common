using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace Gsemac.Net.WebDrivers.Extensions {

    public static class ChromeDriverExtensions {

        public static void AddScriptToEvaluateOnNewDocument(this ChromeDriver webDriver, string script) {

            string commandName = "Page.addScriptToEvaluateOnNewDocument";
            var parameters = new Dictionary<string, object> {
                ["source"] = script
            };

#if EXECUTE_CDP_COMMAND_AVAILABLE
            webDriver.ExecuteCdpCommand(commandName, parameters);
#else
            webDriver.ExecuteChromeCommand(commandName, parameters);
#endif

        }

    }

}
﻿using OpenQA.Selenium;
using System.Linq;

namespace Gsemac.Net.WebDrivers.Extensions {

    public static class WebDriverExtensions {

        public static bool HasQuit(this IWebDriver webDriver) {

            try {

                return !(webDriver?.WindowHandles?.Any()) ?? true;

            }
            catch (WebDriverException) {

                return true;

            }

        }

    }

}
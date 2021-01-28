﻿using Gsemac.Core;
using Gsemac.IO.Logging;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverFactory :
        IFactory<IWebDriver>,
        ILoggable,
        IDisposable {

        IWebDriver Create(IWebBrowserInfo webBrowserInfo);

    }

}
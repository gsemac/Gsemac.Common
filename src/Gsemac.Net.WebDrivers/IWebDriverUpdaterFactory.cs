﻿using Gsemac.Net.WebBrowsers;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverUpdaterFactory {

        IWebDriverUpdater Create(IWebBrowserInfo webBrowserInfo);

    }

}
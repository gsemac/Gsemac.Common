﻿using Gsemac.Net.WebBrowsers;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverFactoryOptions {

        bool AutoUpdateEnabled { get; }
        bool IgnoreUpdateErrors { get; }
        IWebBrowserInfo DefaultWebBrowser { get; }
        bool KillWebDriverProcessesOnDispose { get; }
        string WebDriverDirectoryPath { get; }

    }

}
using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverFileInfo {

        Version Version { get; }
        string ExecutablePath { get; }

    }

}
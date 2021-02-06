using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverVersionInfo {

        Version Version { get; }
        string ExecutablePath { get; }

    }

}
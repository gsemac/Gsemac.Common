using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverInfo {

        Version Version { get; }
        string ExecutablePath { get; }

    }

}
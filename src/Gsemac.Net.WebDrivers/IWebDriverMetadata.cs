using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverMetadata {

        Version Version { get; }
        string ExecutablePath { get; }

    }

}
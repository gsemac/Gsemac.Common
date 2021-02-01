namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverFactoryOptions {

        bool AutoUpdateEnabled { get; }
        bool KillWebDriverProcessesOnDispose { get; }
        string WebDriverDirectory { get; }

    }

}
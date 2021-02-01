namespace Gsemac.Net.WebDrivers {

    public class WebDriverFactoryOptions :
        IWebDriverFactoryOptions {

        public bool AutoUpdateEnabled { get; set; } = true;
        public bool KillWebDriverProcessesOnDispose { get; } = false;
        public string WebDriverDirectory { get; set; }

        public static WebDriverFactoryOptions Default => new WebDriverFactoryOptions();

    }

}
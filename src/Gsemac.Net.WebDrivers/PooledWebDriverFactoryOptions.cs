using System;

namespace Gsemac.Net.WebDrivers {

    public class PooledWebDriverFactoryOptions :
        WebDriverFactoryOptions,
        IPooledWebDriverFactoryOptions {

        public int PoolSize { get; set; } = 2;
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

        public PooledWebDriverFactoryOptions() {
        }
        public PooledWebDriverFactoryOptions(IWebDriverFactoryOptions options) {

            AutoUpdateEnabled = options.AutoUpdateEnabled;
            WebBrowser = options.WebBrowser;
            KillWebDriverProcessesOnDispose = options.KillWebDriverProcessesOnDispose;
            WebDriverDirectoryPath = options.WebDriverDirectoryPath;

        }

        public static new PooledWebDriverFactoryOptions Default => new PooledWebDriverFactoryOptions();

    }

}
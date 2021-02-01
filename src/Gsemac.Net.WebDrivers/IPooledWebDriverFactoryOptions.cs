using System;

namespace Gsemac.Net.WebDrivers {

    public interface IPooledWebDriverFactoryOptions :
        IWebDriverFactoryOptions {

        int PoolSize { get; }
        TimeSpan Timeout { get; }

    }

}
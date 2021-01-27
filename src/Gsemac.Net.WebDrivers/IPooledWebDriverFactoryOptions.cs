using System;

namespace Gsemac.Net.WebDrivers {

    public interface IPooledWebDriverFactoryOptions {

        int PoolSize { get; }
        TimeSpan Timeout { get; }

    }

}
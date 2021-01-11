using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public interface IServiceScope :
        IDisposable {

        IServiceProvider ServiceProvider { get; }

    }

}
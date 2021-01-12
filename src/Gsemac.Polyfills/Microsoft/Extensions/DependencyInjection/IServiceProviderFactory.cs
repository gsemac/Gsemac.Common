using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public interface IServiceProviderFactory<TContainerBuilder> {

        TContainerBuilder CreateBuilder(IServiceCollection services);
        IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder);

    }

}
using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public class DefaultServiceProviderFactory :
        IServiceProviderFactory<IServiceCollection> {

        // Public members

        public DefaultServiceProviderFactory() :
            this(new ServiceProviderOptions()) {

        }
        public DefaultServiceProviderFactory(ServiceProviderOptions options) {

            this.options = options;

        }

        public IServiceCollection CreateBuilder(IServiceCollection services) {

            return services;

        }
        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) {

            return new ServiceProvider(containerBuilder, options);

        }

        // Private members

        private readonly ServiceProviderOptions options;

    }

}
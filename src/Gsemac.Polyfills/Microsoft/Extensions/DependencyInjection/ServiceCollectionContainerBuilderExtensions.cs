using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public static class ServiceCollectionContainerBuilderExtensions {

        public static IServiceProvider BuildServiceProvider(this IServiceCollection collection) {

            return collection.BuildServiceProvider(new ServiceProviderOptions());

        }
        public static IServiceProvider BuildServiceProvider(this IServiceCollection collection, ServiceProviderOptions options) {

            return new ServiceProvider(collection, options);

        }
        public static IServiceProvider BuildServiceProvider(this IServiceCollection collection, bool validateScopes) {

            return collection.BuildServiceProvider(new ServiceProviderOptions() {
                ValidateScopes = validateScopes
            });

        }

    }

}
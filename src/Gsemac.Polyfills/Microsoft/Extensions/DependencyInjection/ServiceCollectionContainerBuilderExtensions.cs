namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public static class ServiceCollectionContainerBuilderExtensions {

        public static ServiceProvider BuildServiceProvider(this IServiceCollection collection) {

            return collection.BuildServiceProvider(new ServiceProviderOptions());

        }
        public static ServiceProvider BuildServiceProvider(this IServiceCollection collection, ServiceProviderOptions options) {

            return new ServiceProvider(collection, options);

        }
        public static ServiceProvider BuildServiceProvider(this IServiceCollection collection, bool validateScopes) {

            return collection.BuildServiceProvider(new ServiceProviderOptions() {
                ValidateScopes = validateScopes
            });

        }

    }

}
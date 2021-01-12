using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public static class ServiceCollectionServiceExtensions {

        public static IServiceCollection AddScoped(this IServiceCollection collection, Type serviceType) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Scoped));

        }
        public static IServiceCollection AddScoped(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, implementationFactory, ServiceLifetime.Scoped));

        }
        public static IServiceCollection AddScoped(this IServiceCollection collection, Type serviceType, Type implementationType) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Scoped));

        }
        public static IServiceCollection AddScoped<TService>(this IServiceCollection collection) {

            return collection.AddScoped(typeof(TService));

        }
        public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory) {

            return collection.AddScoped(typeof(TService), serviceProvider => implementationFactory(serviceProvider));

        }
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection) {

            return collection.AddScoped(typeof(TService), typeof(TImplementation));

        }
        public static IServiceCollection AddSingleton(this IServiceCollection collection, Type serviceType) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Singleton));

        }
        public static IServiceCollection AddSingleton(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, implementationFactory, ServiceLifetime.Singleton));

        }
        public static IServiceCollection AddSingleton(this IServiceCollection collection, Type serviceType, object implementationInstance) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, implementationInstance));

        }
        public static IServiceCollection AddSingleton(this IServiceCollection collection, Type serviceType, Type implementationType) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));

        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection) {

            return collection.AddSingleton(typeof(TService));

        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory) {

            return collection.AddSingleton(typeof(TService), serviceProvider => implementationFactory(serviceProvider));

        }
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection) {

            return collection.AddSingleton(typeof(TService), typeof(TImplementation));

        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, TService implementationInstance) where TService : class {

            return AddSingleton(collection, typeof(TService), implementationInstance);

        }
        public static IServiceCollection AddTransient(this IServiceCollection collection, Type serviceType) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Transient));

        }
        public static IServiceCollection AddTransient(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, implementationFactory, ServiceLifetime.Transient));

        }
        public static IServiceCollection AddTransient(this IServiceCollection collection, Type serviceType, Type implementationType) {

            return ServiceCollectionDescriptorExtensions.Add(collection, new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));

        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection collection) {

            return collection.AddTransient(typeof(TService));

        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory) {

            return collection.AddTransient(typeof(TService), serviceProvider => implementationFactory(serviceProvider));

        }
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection) {

            return collection.AddTransient(typeof(TService), typeof(TImplementation));

        }

    }

}
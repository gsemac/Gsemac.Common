using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public static class ServiceCollectionExtensions {

        public static IServiceCollection Add(this IServiceCollection collection, ServiceDescriptor descriptor) {

            collection.Add(descriptor);

            return collection;

        }
        public static IServiceCollection Add(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors) {

            foreach (ServiceDescriptor serviceDescriptor in descriptors)
                collection.Add(serviceDescriptor);

            return collection;

        }
        public static IServiceCollection RemoveAll(this IServiceCollection collection, Type serviceType) {

            foreach (ServiceDescriptor serviceDescriptor in collection.Where(descriptor => descriptor.ServiceType.Equals(serviceType)).ToArray())
                collection.Remove(serviceDescriptor);

            return collection;

        }
        public static IServiceCollection RemoveAll<T>(this IServiceCollection collection) {

            return collection.RemoveAll(typeof(T));

        }
        public static IServiceCollection Replace(this IServiceCollection collection, ServiceDescriptor descriptor) {

            collection.Remove(collection.Where(serviceDescriptor => serviceDescriptor.ServiceType.Equals(descriptor.ServiceType)).FirstOrDefault());

            return Add(collection, descriptor);

        }
        public static void TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(descriptor.ServiceType)))
                Add(collection, descriptor);

        }
        public static void TryAdd(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors) {

            foreach (ServiceDescriptor descriptor in descriptors)
                collection.TryAdd(descriptor);

        }
        public static void TryAddEnumerable(this IServiceCollection collection, ServiceDescriptor descriptor) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(descriptor.ServiceType) && serviceDescriptor.ImplementationType.Equals(descriptor.ImplementationType)))
                Add(collection, descriptor);

        }
        public static void TryAddEnumerable(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors) {

            foreach (ServiceDescriptor descriptor in descriptors)
                collection.TryAddEnumerable(descriptor);

        }
        public static void TryAddScoped(this IServiceCollection collection, Type service) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddScoped(service);

        }
        public static void TryAddScoped(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddScoped(service, implementationFactory);

        }
        public static void TryAddScoped(this IServiceCollection collection, Type service, Type implementationType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddScoped(service, implementationType);

        }
        public static void TryAddScoped<TService>(this IServiceCollection collection) {

            collection.TryAddScoped(typeof(TService));

        }
        public static void TryAddScoped<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory) {

            collection.TryAddScoped(typeof(TService), serviceProvider => implementationFactory(serviceProvider));

        }
        public static void TryAddScoped<TService, TImplementation>(this IServiceCollection collection) {

            collection.TryAddScoped(typeof(TService), typeof(TImplementation));

        }
        public static void TryAddSingleton(this IServiceCollection collection, Type service) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddSingleton(service);

        }
        public static void TryAddSingleton(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddSingleton(service, implementationFactory);

        }
        public static void TryAddSingleton(this IServiceCollection collection, Type service, Type implementationType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddSingleton(service, implementationType);

        }
        public static void TryAddSingleton<TService>(this IServiceCollection collection) {

            collection.TryAddSingleton(typeof(TService));

        }
        public static void TryAddSingleton<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory) {

            collection.TryAddSingleton(typeof(TService), serviceProvider => implementationFactory(serviceProvider));

        }
        public static void TryAddSingleton<TService, TImplementation>(this IServiceCollection collection) {

            collection.TryAddSingleton(typeof(TService), typeof(TImplementation));

        }
        public static void TryAddTransient(this IServiceCollection collection, Type service) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddTransient(service);

        }
        public static void TryAddTransient(this IServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddTransient(service, implementationFactory);

        }
        public static void TryAddTransient(this IServiceCollection collection, Type service, Type implementationType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(service)))
                collection.AddTransient(service, implementationType);

        }
        public static void TryAddTransient<TService>(this IServiceCollection collection) {

            collection.TryAddTransient(typeof(TService));

        }
        public static void TryAddTransient<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> implementationFactory) {

            collection.TryAddTransient(typeof(TService), serviceProvider => implementationFactory(serviceProvider));

        }
        public static void TryAddTransient<TService, TImplementation>(this IServiceCollection collection) {

            collection.TryAddTransient(typeof(TService), typeof(TImplementation));

        }
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
        public static IServiceCollection AddScoped(this IServiceCollection collection, Type serviceType) {

            return Add(collection, new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Scoped));

        }
        public static IServiceCollection AddScoped(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            return Add(collection, new ServiceDescriptor(serviceType, implementationFactory, ServiceLifetime.Scoped));

        }
        public static IServiceCollection AddScoped(this IServiceCollection collection, Type serviceType, Type implementationType) {

            return Add(collection, new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Scoped));

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

            return Add(collection, new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Singleton));

        }
        public static IServiceCollection AddSingleton(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            return Add(collection, new ServiceDescriptor(serviceType, implementationFactory, ServiceLifetime.Singleton));

        }
        public static IServiceCollection AddSingleton(this IServiceCollection collection, Type serviceType, Type implementationType) {

            return Add(collection, new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));

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
        public static IServiceCollection AddTransient(this IServiceCollection collection, Type serviceType) {

            return Add(collection, new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Transient));

        }
        public static IServiceCollection AddTransient(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            return Add(collection, new ServiceDescriptor(serviceType, implementationFactory, ServiceLifetime.Transient));

        }
        public static IServiceCollection AddTransient(this IServiceCollection collection, Type serviceType, Type implementationType) {

            return Add(collection, new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));

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
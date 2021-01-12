using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public static class ServiceCollectionDescriptorExtensions {

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
        public static void TryAddScoped(this IServiceCollection collection, Type serviceType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddScoped(serviceType);

        }
        public static void TryAddScoped(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddScoped(serviceType, implementationFactory);

        }
        public static void TryAddScoped(this IServiceCollection collection, Type serviceType, Type implementationType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddScoped(serviceType, implementationType);

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
        public static void TryAddSingleton(this IServiceCollection collection, Type serviceType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddSingleton(serviceType);

        }
        public static void TryAddSingleton(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddSingleton(serviceType, implementationFactory);

        }
        public static void TryAddSingleton(this IServiceCollection collection, Type serviceType, Type implementationType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddSingleton(serviceType, implementationType);

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
        public static void TryAddTransient(this IServiceCollection collection, Type serviceType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddTransient(serviceType);

        }
        public static void TryAddTransient(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddTransient(serviceType, implementationFactory);

        }
        public static void TryAddTransient(this IServiceCollection collection, Type serviceType, Type implementationType) {

            if (!collection.Any(serviceDescriptor => serviceDescriptor.ServiceType.Equals(serviceType)))
                collection.AddTransient(serviceType, implementationType);

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

    }

}
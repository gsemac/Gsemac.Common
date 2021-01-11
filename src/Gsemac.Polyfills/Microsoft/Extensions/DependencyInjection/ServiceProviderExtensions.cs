using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public static class ServiceProviderExtensions {

        public static IServiceScope CreateScope(this IServiceProvider provider) {

            return provider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        }
        public static object GetRequiredService(this IServiceProvider provider, Type serviceType) {

            object serviceObject = provider.GetService(serviceType);

            if (serviceObject is null)
                throw new InvalidOperationException($"There is no service of type {serviceType?.ToString() ?? "null"}.");

            return serviceObject;

        }
        public static T GetRequiredService<T>(this IServiceProvider provider) {

            return (T)GetRequiredService(provider, typeof(T));

        }
        public static T GetService<T>(this IServiceProvider provider) {

            return (T)provider.GetService(typeof(T));

        }
        public static IEnumerable<object> GetServices(this IServiceProvider provider, Type serviceType) {

            List<object> services = new List<object>();

            object serviceObject = provider.GetService(serviceType);

            if (serviceObject is object)
                services.Add(serviceObject);

            return services;

        }
        public static IEnumerable<T> GetServices<T>(this IServiceProvider provider) {

            return provider.GetServices(typeof(T)).OfType<T>();

        }

    }

}
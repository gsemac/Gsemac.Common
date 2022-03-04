using Gsemac.Polyfills.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public static class ServiceProviderExtensions {

        public static IServiceScope CreateScope(this IServiceProvider provider) {

            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            return provider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        }
        public static object GetRequiredService(this IServiceProvider provider, Type serviceType) {

            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));

            object serviceObject = provider.GetService(serviceType);

            if (serviceObject is null)
                throw new InvalidOperationException(string.Format(ExceptionMessages.CannotResolveScopedServiceFromRootProviderWithTypeName, serviceType?.ToString() ?? "null"));

            return serviceObject;

        }
        public static T GetRequiredService<T>(this IServiceProvider provider) {

            return (T)GetRequiredService(provider, typeof(T));

        }
        public static T GetService<T>(this IServiceProvider provider) {

            return (T)provider.GetService(typeof(T));

        }
        public static IEnumerable<object> GetServices(this IServiceProvider provider, Type serviceType) {

            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));

            Type enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);

            // Return the services as an array (this is in line with Microsoft's implementation).

            return ((IEnumerable<object>)provider.GetService(enumerableType)).ToArray();

        }
        public static IEnumerable<T> GetServices<T>(this IServiceProvider provider) {

            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            return provider.GetServices(typeof(T)).OfType<T>();

        }

    }

}
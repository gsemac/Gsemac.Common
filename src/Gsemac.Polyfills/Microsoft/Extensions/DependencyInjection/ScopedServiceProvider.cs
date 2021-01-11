using System;
using System.Collections.Generic;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    internal sealed class ScopedServiceProvider :
        IServiceProvider,
        IDisposable {

        // Public members

        public ScopedServiceProvider(ServiceProvider serviceProvider) {

            this.serviceProvider = serviceProvider;

        }

        public object GetService(Type serviceType) {

            if (disposedValue)
                throw new ObjectDisposedException(nameof(ScopedServiceProvider));

            lock (scopedServicesDictMutex) {

                if (scopedServicesDict.TryGetValue(serviceType, out object scopedServiceObject))
                    return scopedServiceObject;

                ServiceDescriptor serviceDescriptor = serviceProvider.GetServiceDescriptor(serviceType);
                object serviceObject = serviceDescriptor?.ImplementationFactory(serviceProvider);

                if (serviceObject is object && serviceDescriptor.Lifetime == ServiceLifetime.Scoped)
                    scopedServicesDict[serviceType] = serviceObject;

                return serviceObject;

            }

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Private members

        private bool disposedValue;
        private readonly ServiceProvider serviceProvider;
        private readonly object scopedServicesDictMutex = new object();
        private readonly IDictionary<Type, object> scopedServicesDict = new Dictionary<Type, object>();

        private void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    foreach (object service in scopedServicesDict.Values)
                        if (service is IDisposable disposable)
                            disposable.Dispose();

                }

                disposedValue = true;
            }

        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public sealed class ServiceProvider :
        IServiceProvider,
        IDisposable {

        // Public members

        internal ServiceProvider(IServiceCollection services, ServiceProviderOptions options) {

            foreach (ServiceDescriptor serviceDescriptor in services.Concat(new[] {
                new ServiceDescriptor(typeof(IServiceScopeFactory), new ServiceScopeFactory(this))
            })) {

                if (!servicesDict.ContainsKey(serviceDescriptor.ServiceType))
                    servicesDict.Add(serviceDescriptor.ServiceType, new List<ServiceDescriptor>());

                servicesDict[serviceDescriptor.ServiceType].Add(serviceDescriptor);

            }

            // Validate services by making sure all of them can be instantiated.

            if (options.ValidateOnBuild)
                foreach (Type serviceType in servicesDict.Keys)
                    this.GetRequiredService(serviceType);

        }

        public object GetService(Type serviceType) {

            return GetServiceDescriptor(serviceType)?.ImplementationFactory(this);

        }
        internal ServiceDescriptor GetServiceDescriptor(Type serviceType) {

            if (servicesDict.TryGetValue(serviceType, out IList<ServiceDescriptor> descriptors))
                return descriptors.FirstOrDefault();

            return null;

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        private void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    // Dispose of any disposable singletons.

                    foreach (var serviceDescriptors in servicesDict.Values)
                        foreach (ServiceDescriptor descriptor in serviceDescriptors)
                            if (descriptor.ImplementationInstance is IDisposable disposable)
                                disposable.Dispose();

                }

                disposedValue = true;
            }

        }

        // Private members

        private bool disposedValue;

        private readonly IDictionary<Type, IList<ServiceDescriptor>> servicesDict = new Dictionary<Type, IList<ServiceDescriptor>>();

    }

}
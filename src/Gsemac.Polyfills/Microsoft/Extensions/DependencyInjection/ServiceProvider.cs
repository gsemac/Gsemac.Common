﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public sealed class ServiceProvider :
        IServiceProvider,
        IDisposable {

        // Internal members

        internal ServiceProvider(IServiceCollection services, ServiceProviderOptions options) {

            foreach (ServiceDescriptor serviceDescriptor in services.Concat(new[] {
                new ServiceDescriptor(typeof(IServiceScopeFactory), new ServiceScopeFactory(this))
            })) {

                if (!servicesDict.ContainsKey(serviceDescriptor.ServiceType))
                    servicesDict.Add(serviceDescriptor.ServiceType, new List<ServiceDescriptor>());

                servicesDict[serviceDescriptor.ServiceType].Add(serviceDescriptor);

            }

            if (options.ValidateOnBuild)
                ValidateServices();

            // If we allow scoped services to be resolved from the root provider, they'll treated as singletons under the root scope.

            validateScopes = options.ValidateScopes;

            if (!validateScopes)
                rootScope = this.CreateScope();

        }

        internal ServiceDescriptor GetServiceDescriptor(Type serviceType) {

            if (disposedValue)
                throw new ObjectDisposedException(nameof(ServiceProvider));

            // Note: No need to lock here, since the services dictionary will never be modified after it is created.

            if (servicesDict.TryGetValue(serviceType, out IList<ServiceDescriptor> descriptors))
                return descriptors.FirstOrDefault();

            return null;

        }

        // Public members

        public object GetService(Type serviceType) {

            if (disposedValue)
                throw new ObjectDisposedException(nameof(ServiceProvider));

            if (serviceType == typeof(IServiceProvider))
                return this;

            ServiceDescriptor serviceDescriptor = GetServiceDescriptor(serviceType);

            if (serviceDescriptor is object && serviceDescriptor.Lifetime == ServiceLifetime.Scoped) {

                if (validateScopes) {

                    // Scoped services must be accessed from within a scope.

                    throw new InvalidOperationException($"Cannot resolve scoped service '{serviceType}' from root provider.");

                }
                else {

                    // Scoped services are treated as singletons under the root scope.

                    return rootScope.ServiceProvider.GetService(serviceType);

                }

            }

            return serviceDescriptor?.ImplementationFactory(this);

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

                    // Dispose of the root scope, disposing of any instantiated scoped services.

                    rootScope?.Dispose();

                }

                disposedValue = true;
            }

        }

        // Private members

        private bool disposedValue;

        private readonly bool validateScopes = false;
        private readonly IServiceScope rootScope;
        private readonly IDictionary<Type, IList<ServiceDescriptor>> servicesDict = new Dictionary<Type, IList<ServiceDescriptor>>();

        private void ValidateServices() {

            // Validate services by making sure all of them can be instantiated.

            foreach (Type serviceType in servicesDict.Keys)
                this.GetRequiredService(serviceType);

        }

    }

}
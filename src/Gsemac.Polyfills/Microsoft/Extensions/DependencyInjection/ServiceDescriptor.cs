using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public class ServiceDescriptor {

        // Public members

        public Func<IServiceProvider, object> ImplementationFactory { get; }
        public object ImplementationInstance { get; private set; }
        public Type ImplementationType { get; }
        public ServiceLifetime Lifetime { get; }
        public Type ServiceType { get; }

        public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime) {

            ImplementationFactory = factory;
            ImplementationInstance = null;
            ImplementationType = serviceType;
            ServiceType = serviceType;
            Lifetime = lifetime;

        }
        public ServiceDescriptor(Type serviceType, object instance) {

            ImplementationFactory = DefaultImplementationFactory;
            ImplementationType = serviceType;
            ServiceType = serviceType;
            Lifetime = ServiceLifetime.Singleton;
            ImplementationInstance = instance;

        }
        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime) {

            ImplementationFactory = DefaultImplementationFactory;
            ImplementationInstance = null;
            ImplementationType = implementationType;
            ServiceType = serviceType;
            Lifetime = lifetime;

        }

        // Private members

        private object DefaultImplementationFactory(IServiceProvider serviceProvider) {

            // Create the default implementation instance if we haven't already (for singletons only).

            if (ImplementationInstance is null && Lifetime == ServiceLifetime.Singleton)
                ImplementationInstance = CreateInstance(serviceProvider);

            // If we have an instance, return it.

            if (ImplementationInstance is object)
                return ImplementationInstance;

            // If we don't have an instance, this must not be a singleton.

            return CreateInstance(serviceProvider);

        }
        private object CreateInstance(IServiceProvider serviceProvider) {

            return ImplementationFactory is object && ImplementationFactory != DefaultImplementationFactory ?
                ImplementationFactory(serviceProvider) :
                ActivatorUtilities.CreateInstance(serviceProvider, ServiceType);

        }

    }

}
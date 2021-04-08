using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public class ServiceDescriptor {

        // Public members

        public Func<IServiceProvider, object> ImplementationFactory => InternalImplementationFactory;
        public object ImplementationInstance { get; private set; }
        public Type ImplementationType { get; }
        public ServiceLifetime Lifetime { get; }
        public Type ServiceType { get; }

        public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime) {

            implementationFactory = factory;
            ImplementationInstance = null;
            ImplementationType = serviceType;
            ServiceType = serviceType;
            Lifetime = lifetime;

        }
        public ServiceDescriptor(Type serviceType, object instance) {

            ImplementationType = serviceType;
            ServiceType = serviceType;
            Lifetime = ServiceLifetime.Singleton;
            ImplementationInstance = instance;

        }
        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime) {

            ImplementationInstance = null;
            ImplementationType = implementationType;
            ServiceType = serviceType;
            Lifetime = lifetime;

        }

        // Private members

        private readonly object singletonInstantiationMutex = new object();
        private readonly Func<IServiceProvider, object> implementationFactory;

        private object InternalImplementationFactory(IServiceProvider serviceProvider) {

            // Create the default implementation instance if we haven't already (for singletons only).
            // Make sure that even if the service descriptor is being used by multiple threads that the service is only instantiated once.

            if (ImplementationInstance is null) {

                lock (singletonInstantiationMutex) {

                    if (ImplementationInstance is null && Lifetime == ServiceLifetime.Singleton)
                        ImplementationInstance = CreateInstance(serviceProvider);

                }

            }

            // If we have an instance, return it.

            if (ImplementationInstance is object)
                return ImplementationInstance;

            // If we don't have an instance, this must not be a singleton.

            return CreateInstance(serviceProvider);

        }
        private object CreateInstance(IServiceProvider serviceProvider) {

            return implementationFactory is object ?
                implementationFactory(serviceProvider) :
                ActivatorUtilities.CreateInstance(serviceProvider, ImplementationType);

        }

    }

}
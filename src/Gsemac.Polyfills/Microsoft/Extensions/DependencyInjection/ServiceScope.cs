using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    internal sealed class ServiceScope :
         IServiceScope {

        // Public members

        public IServiceProvider ServiceProvider => scopedServiceProvider;

        public ServiceScope(ServiceProvider serviceProvider) {

            scopedServiceProvider = new ScopedServiceProvider(serviceProvider);

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Private members

        private bool disposedValue;
        private readonly ScopedServiceProvider scopedServiceProvider;

        private void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    scopedServiceProvider.Dispose();

                }

                disposedValue = true;
            }

        }

    }

}
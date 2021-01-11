using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    internal sealed class ServiceScopeFactory :
      IServiceScopeFactory {

        // Public members

        public ServiceScopeFactory(ServiceProvider serviceProvider) {

            this.serviceProvider = serviceProvider;

        }

        public IServiceScope CreateScope() {

            return new ServiceScope(serviceProvider);

        }

        // Private members

        private readonly ServiceProvider serviceProvider;

    }

}
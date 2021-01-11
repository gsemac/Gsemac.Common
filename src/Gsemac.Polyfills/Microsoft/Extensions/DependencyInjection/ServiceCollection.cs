using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public class ServiceCollection :
        Collection<ServiceDescriptor>,
        IServiceCollection {

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item) {

            Add(item);

        }

    }

}
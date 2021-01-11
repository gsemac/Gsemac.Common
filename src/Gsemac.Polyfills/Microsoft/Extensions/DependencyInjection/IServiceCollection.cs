using System.Collections.Generic;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    public interface IServiceCollection :
        IList<ServiceDescriptor> {
    }

}
using System;

namespace Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection {

    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class ActivatorUtilitiesConstructorAttribute :
        Attribute {
    }

}
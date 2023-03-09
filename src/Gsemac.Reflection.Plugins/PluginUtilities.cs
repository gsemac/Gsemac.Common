using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Reflection.Plugins {

    public static class PluginUtilities {

        // Public members

        public static bool TestRequirementAttributes(Type type, IServiceProvider serviceProvider) {

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            // By providing a default service provider, the attributes themselves don't necessarily need to test for null.

            if (serviceProvider is null)
                serviceProvider = new NullServiceProvider();

            IEnumerable<IRequirementAttribute> requirements =
                Attribute.GetCustomAttributes(type).OfType<IRequirementAttribute>();

            return requirements.All(requirement => requirement.TestRequirement(serviceProvider));

        }

    }

}
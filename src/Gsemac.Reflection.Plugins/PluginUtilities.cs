using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Reflection.Plugins {

    public static class PluginUtilities {

        public static bool TestRequirementAttributes(Type type, IServiceProvider serviceProvider) {

            IEnumerable<IRequirementAttribute> requirements =
                Attribute.GetCustomAttributes(type).OfType<IRequirementAttribute>();

            return requirements.All(requirement => requirement.TestRequirement(serviceProvider));

        }

    }

}
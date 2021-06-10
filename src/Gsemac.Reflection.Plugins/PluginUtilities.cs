using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Reflection.Plugins {

    public static class PluginUtilities {

        public static bool TestRequirementAttributes(Type type) {

            IEnumerable<IRequirementAttribute> requirements =
                Attribute.GetCustomAttributes(type).OfType<IRequirementAttribute>();

            return requirements.All(requirement => requirement.IsSatisfied);

        }

    }

}
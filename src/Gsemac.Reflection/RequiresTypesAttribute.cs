using System;
using System.Linq;

namespace Gsemac.Reflection {

    public sealed class RequiresTypesAttribute :
        Attribute,
        IRequirementAttribute {

        // Public members

        public bool IsSatisfied => typeRequirementSatisfied.Value;

        public RequiresTypesAttribute(params string[] requiredTypeNames) {

            this.requiredTypeNames = requiredTypeNames;
            this.typeRequirementSatisfied = new Lazy<bool>(CheckTypeRequirement);

        }

        // Private members

        private readonly string[] requiredTypeNames;
        private readonly Lazy<bool> typeRequirementSatisfied;

        private bool CheckTypeRequirement() {

            // This requirement will only ever be checked once in the event the containing assembly does not exist.

            return requiredTypeNames.All(typeName => TypeUtilities.TypeExists(typeName));

        }

    }

}
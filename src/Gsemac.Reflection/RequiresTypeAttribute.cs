using System;

namespace Gsemac.Reflection {

    public sealed class RequiresTypeAttribute :
        Attribute,
        IRequirementAttribute {

        // Public members

        public bool IsSatisfied => typeRequirementSatisfied.Value;

        public RequiresTypeAttribute(string requiredTypeName) {

            this.requiredTypeName = requiredTypeName;
            this.typeRequirementSatisfied = new Lazy<bool>(CheckTypeRequirement);

        }

        // Private members

        private readonly string requiredTypeName;
        private readonly Lazy<bool> typeRequirementSatisfied;

        private bool CheckTypeRequirement() {

            // This requirement will only ever be checked once in the event the containing assembly does not exist.

            return TypeUtilities.TypeExists(requiredTypeName);

        }

    }

}
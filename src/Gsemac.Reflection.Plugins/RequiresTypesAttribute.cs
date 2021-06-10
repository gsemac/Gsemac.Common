using System;
using System.Linq;

namespace Gsemac.Reflection.Plugins {

    public sealed class RequiresTypesAttribute :
        RequirementAttributeBase {

        // Public members

        public bool IsSatisfied => typeRequirementSatisfied.Value;

        public RequiresTypesAttribute(params string[] requiredTypeNames) {

            this.requiredTypeNames = requiredTypeNames;
            typeRequirementSatisfied = new Lazy<bool>(TestRequirementInternal);

        }

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            return typeRequirementSatisfied.Value;

        }

        // Private members

        private readonly string[] requiredTypeNames;
        private readonly Lazy<bool> typeRequirementSatisfied;

        private bool TestRequirementInternal() {

            // This requirement will only ever be checked once in the event the containing assembly does not exist.

            return requiredTypeNames.All(typeName => TypeUtilities.TypeExists(typeName));

        }

    }

}
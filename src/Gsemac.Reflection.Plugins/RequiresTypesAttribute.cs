using System;
using System.Linq;

namespace Gsemac.Reflection.Plugins {

    public sealed class RequiresTypesAttribute :
        RequirementAttributeBase {

        // Public members

        public RequiresTypesAttribute(params string[] requiredTypeNames) {

            if (requiredTypeNames is null)
                throw new ArgumentNullException(nameof(requiredTypeNames));

            this.requiredTypeNames = requiredTypeNames;
            result = new Lazy<bool>(TestRequirementInternal);

        }

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            return result.Value;

        }

        // Private members

        private readonly string[] requiredTypeNames;
        private readonly Lazy<bool> result;

        private bool TestRequirementInternal() {

            // This requirement will only ever be checked once in the event the containing assembly does not exist.

            return requiredTypeNames.All(typeName => TypeUtilities.TypeExists(typeName));

        }

    }

}
using System;
using System.Linq;

namespace Gsemac.Reflection.Plugins {

    public abstract class RequirementAttributeBase :
        Attribute,
        IRequirementAttribute {

        // Public members

        public virtual bool TestRequirement(IServiceProvider serviceProvider) {

            if (requirements is null || requirements.Length <= 0)
                return true;

            if (serviceProvider is null)
                serviceProvider = new NullServiceProvider();

            return requirements.All(requirement => TestRequirement(serviceProvider));

        }

        // Protected members

        protected RequirementAttributeBase() {
        }
        protected RequirementAttributeBase(IRequirementAttribute requirement) {

            if (requirement is null)
                throw new ArgumentNullException(nameof(requirement));

            requirements = new[] {
                requirement
            };

        }
        protected RequirementAttributeBase(IRequirementAttribute[] requirements) {

            if (requirements is null)
                throw new ArgumentNullException(nameof(requirements));

            this.requirements = requirements;

        }

        // Private members

        private readonly IRequirementAttribute[] requirements;

    }

}
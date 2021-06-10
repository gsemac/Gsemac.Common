using System;

namespace Gsemac.Reflection.Plugins {

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class RequiresAssemblyOrTypesAttribute :
        RequirementAttributeBase {

        // Public members

        public RequiresAssemblyOrTypesAttribute(string containingAssemblyName, params string[] requiredTypeNames) {

            assemblyRequirement = new RequiresAssembliesAttribute(containingAssemblyName);
            typeRequirement = new RequiresTypesAttribute(requiredTypeNames);

        }

        public override bool TestRequirement(IServiceProvider serviceProvider) {

            return assemblyRequirement.TestRequirement(serviceProvider) || typeRequirement.TestRequirement(serviceProvider);

        }

        // Private members

        private readonly RequiresAssembliesAttribute assemblyRequirement;
        private readonly RequiresTypesAttribute typeRequirement;

    }

}
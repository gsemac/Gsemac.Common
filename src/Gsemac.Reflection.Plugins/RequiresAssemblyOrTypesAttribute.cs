using System;

namespace Gsemac.Reflection.Plugins {

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class RequiresAssemblyOrTypesAttribute :
        Attribute,
        IRequirementAttribute {

        // Public members

        public bool IsSatisfied => assemblyRequirement.IsSatisfied || typeRequirement.IsSatisfied;

        public RequiresAssemblyOrTypesAttribute(string containingAssemblyName, params string[] requiredTypeNames) {

            assemblyRequirement = new RequiresAssembliesAttribute(containingAssemblyName);
            typeRequirement = new RequiresTypesAttribute(requiredTypeNames);

        }

        // Private members

        private readonly RequiresAssembliesAttribute assemblyRequirement;
        private readonly RequiresTypesAttribute typeRequirement;

    }

}
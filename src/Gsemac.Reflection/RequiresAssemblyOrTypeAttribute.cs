using System;

namespace Gsemac.Reflection {

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class RequiresAssemblyOrTypeAttribute :
        Attribute,
        IRequirementAttribute {

        // Public members

        public bool IsSatisfied => assemblyRequirement.IsSatisfied || typeRequirement.IsSatisfied;

        public RequiresAssemblyOrTypeAttribute(string containingAssemblyName, string requiredTypeName) {

            assemblyRequirement = new RequiresAssembliesAttribute(containingAssemblyName);
            typeRequirement = new RequiresTypeAttribute(requiredTypeName);

        }

        // Private members

        private readonly RequiresAssembliesAttribute assemblyRequirement;
        private readonly RequiresTypeAttribute typeRequirement;

    }

}
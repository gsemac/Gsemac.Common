using System;

namespace Gsemac.Reflection {

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RequiresAssembliesAttribute :
        Attribute,
        IRequirementAttribute {

        // Public members

        public bool IsSatisfied => CheckRequirement();

        public bool X86 { get; set; } = false;
        public bool X64 { get; set; } = false;

        public RequiresAssembliesAttribute(params string[] requiredAssemblyNames) {

            this.requiredAssemblyNames = requiredAssemblyNames;

        }

        // Private members

        private readonly string[] requiredAssemblyNames;

        private bool CheckRequirement() {

            if ((X86 || X64) && !(X86 && X64)) {

                bool isX64 = Environment.Is64BitProcess;
                bool isX86 = !isX64;

                if (X86 && !isX86)
                    return true;

                if (X64 && !isX64)
                    return true;

            }

            foreach (string assemblyName in requiredAssemblyNames) {

                if (!AssemblyResolver.Default.AssemblyExists(assemblyName))
                    return false;

            }

            return true;

        }

    }

}
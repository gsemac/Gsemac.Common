using System;
using System.Reflection;

namespace Gsemac.Reflection {

    public static class AssemblyResolverExtensions {

        // Public members

        public static bool TryResolveAssembly(this IAssemblyResolver assemblyResolver, string assemblyName, out Assembly result) {

            result = null;

            if (assemblyResolver is null)
                throw new ArgumentNullException(nameof(assemblyResolver));

            result = assemblyResolver.ResolveAssembly(assemblyName);

            if (result is null)
                return false;

            return true;

        }

    }

}
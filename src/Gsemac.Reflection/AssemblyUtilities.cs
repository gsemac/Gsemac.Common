using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Reflection {

    public static class AssemblyUtilities {

        // Public members

        public static bool LoadReferencedAssemblies() {

            bool success = true;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                success &= LoadReferencedAssemblies(assembly);

            return success;

        }
        public static bool LoadReferencedAssemblies(Assembly assembly) {

            return TryLoadReferencedAssemblies(assembly, ignoreExceptions: false, out _);

        }
        public static bool TryLoadReferencedAssemblies(out IEnumerable<AssemblyName> failedAssemblies) {

            failedAssemblies = Enumerable.Empty<AssemblyName>();

            bool success = true;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                success &= TryLoadReferencedAssemblies(assembly, out failedAssemblies);

            return success;

        }
        public static bool TryLoadReferencedAssemblies(Assembly assembly, out IEnumerable<AssemblyName> failedAssemblies) {

            return TryLoadReferencedAssemblies(assembly, ignoreExceptions: true, out failedAssemblies);

        }

        // Private members

        private static bool TryLoadReferencedAssemblies(Assembly assembly, bool ignoreExceptions, out IEnumerable<AssemblyName> failedAssemblies) {

            IEnumerable<AssemblyName> referencedAssemblies = assembly.GetReferencedAssemblies()
                .Except(AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName()))
                .Distinct();

            List<AssemblyName> failedAssembliesList = new List<AssemblyName>();

            foreach (AssemblyName assemblyName in referencedAssemblies) {

                try {

                    Assembly.Load(assemblyName);

                }
                catch (Exception ex) {

                    failedAssembliesList.Add(assemblyName);

                    if (!ignoreExceptions)
                        throw ex;

                }

            }

            failedAssemblies = failedAssembliesList;

            return !failedAssembliesList.Any();

        }

    }

}

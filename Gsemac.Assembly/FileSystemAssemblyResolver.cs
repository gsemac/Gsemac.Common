using System;
using System.Collections.Generic;

namespace Gsemac.Assembly {

    public class FileSystemAssemblyResolver :
        IAssemblyResolver {

        // Public members

        public bool AddExtension { get; set; } = true;
        public ICollection<string> ProbingPaths { get; } = new List<string>();
        public bool ProbeCurrentDirectory { get; set; } = true;
        public bool Unsafe { get; set; } = false;

        public System.Reflection.Assembly ResolveAssembly(string assemblyName) {

            string assemblyPath = GetAssemblyPath(assemblyName);

            if (!string.IsNullOrEmpty(assemblyPath)) {

                if (Unsafe)
                    return System.Reflection.Assembly.UnsafeLoadFrom(assemblyPath);
                else
                    return System.Reflection.Assembly.LoadFrom(assemblyPath);

            }

            return null;

        }
        public System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs e) {

            return ResolveAssembly(new System.Reflection.AssemblyName(e.Name).Name);

        }

        public bool AssemblyExists(string assemblyName) {

            return !string.IsNullOrEmpty(GetAssemblyPath(assemblyName));

        }
        public string GetAssemblyPath(string assemblyName) {

            string assemblyPath = string.Empty;

            // Attempt to find the assembly relative to one of the probing paths.

            foreach (string probingPath in ProbingPaths) {

                string candidatePath = probingPath;

                if (AddExtension && !assemblyName.ToLowerInvariant().EndsWith(".dll"))
                    assemblyName += ".dll";

                // If the probing path is relative, make it rooted relative to the entry assembly.

                if (!string.IsNullOrEmpty(candidatePath) && !System.IO.Path.IsPathRooted(candidatePath))
                    candidatePath = System.IO.Path.Combine(new EntryAssemblyInfo().Directory, candidatePath);

                // Attempt to find the assembly in the probing path.

                if (!string.IsNullOrEmpty(candidatePath)) {

                    candidatePath = System.IO.Path.Combine(candidatePath, assemblyName);

                    if (System.IO.File.Exists(candidatePath)) {

                        assemblyPath = candidatePath;

                        break;

                    }

                }

            }

            // Attempt to find the assembly next to the entry assembly.

            if (ProbeCurrentDirectory) {

                string candidatePath = System.IO.Path.Combine(new EntryAssemblyInfo().Directory, assemblyName);

                if (System.IO.File.Exists(candidatePath))
                    assemblyPath = candidatePath;

            }

            return assemblyPath;

        }

    }

}
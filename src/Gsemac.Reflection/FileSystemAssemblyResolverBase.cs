using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Gsemac.Reflection {

    public abstract class FileSystemAssemblyResolverBase :
        IFileSystemAssemblyResolver {

        // Public members

        public bool AddExtension { get; set; } = true;
        public ICollection<string> ProbingPaths { get; } = new List<string>();
        public bool Unsafe { get; set; } = false;

        public Assembly ResolveAssembly(string assemblyName) {

            string assemblyPath = GetAssemblyPath(assemblyName);

            if (!string.IsNullOrEmpty(assemblyPath)) {

                if (Unsafe)
                    return Assembly.UnsafeLoadFrom(assemblyPath);
                else
                    return Assembly.LoadFrom(assemblyPath);

            }

            return null;

        }
        public Assembly ResolveAssembly(object sender, ResolveEventArgs e) {

            return ResolveAssembly(new AssemblyName(e.Name).Name);

        }

        public bool AssemblyExists(string assemblyName) {

            return !string.IsNullOrEmpty(GetAssemblyPath(assemblyName));

        }

        public virtual string GetAssemblyPath(string assemblyName) {

            string assemblyPath = string.Empty;

            // Attempt to find the assembly relative to one of the probing paths.

            foreach (string probingPath in GetProbingPaths()) {

                string candidatePath = probingPath;

                if (AddExtension && !assemblyName.ToLowerInvariant().EndsWith(".dll"))
                    assemblyName += ".dll";

                // If the probing path is relative, make it rooted relative to the entry assembly.

                if (!string.IsNullOrEmpty(candidatePath) && !Path.IsPathRooted(candidatePath))
                    candidatePath = Path.Combine(AssemblyInfo.EntryAssembly.Directory, candidatePath);

                // Attempt to find the assembly in the probing path.

                if (!string.IsNullOrEmpty(candidatePath)) {

                    candidatePath = Path.Combine(candidatePath, assemblyName);

                    if (File.Exists(candidatePath)) {

                        assemblyPath = candidatePath;

                        break;

                    }

                }

            }

            return assemblyPath;

        }
        public IEnumerable<string> GetAssemblyPaths(string searchPattern = "*.dll") {

            List<string> assemblyPaths = new List<string>();

            foreach (string probingPath in GetProbingPaths())
                if (Directory.Exists(probingPath))
                    assemblyPaths.AddRange(Directory.GetFiles(probingPath, searchPattern, SearchOption.TopDirectoryOnly));

            return assemblyPaths;

        }

        // Protected members

        protected virtual IEnumerable<string> GetProbingPaths() {

            // EntryAssembly can return null in certain contexts (e.g. if we're running under a unit testing framework).
            // In that case, CallingAssembly is equivalent (https://stackoverflow.com/a/53228317).

            return ProbingPaths.Concat(new[] {
                Directory.GetCurrentDirectory(),
                (AssemblyInfo.EntryAssembly ?? AssemblyInfo.CallingAssembly).Directory,
            }).Distinct().Select(path => Path.GetFullPath(path));

        }

    }

}
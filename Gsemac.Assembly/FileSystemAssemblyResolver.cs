namespace Gsemac.Assembly {

    public class FileSystemAssemblyResolver :
        FileSystemAssemblyResolverBase {

        // Public members

        public override string GetAssemblyPath(string assemblyName) {

            string assemblyPath = string.Empty;

            // Attempt to find the assembly relative to one of the probing paths.

            foreach (string probingPath in GetProbingPaths()) {

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
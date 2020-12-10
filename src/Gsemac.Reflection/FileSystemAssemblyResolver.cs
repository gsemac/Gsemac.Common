using System.Collections.Generic;

namespace Gsemac.Reflection {

    public class FileSystemAssemblyResolver :
        FileSystemAssemblyResolverBase {

        // Public members

        public FileSystemAssemblyResolver() :
            this(new[] { "lib" }) {
        }
        public FileSystemAssemblyResolver(IEnumerable<string> probingPaths) {

            foreach (string probingPath in probingPaths)
                ProbingPaths.Add(probingPath);

        }

    }

}
using System;
using System.Collections.Generic;

namespace Gsemac.Reflection {

    public class FileSystemAssemblyResolver :
        FileSystemAssemblyResolverBase {

        // Public members

        public static IFileSystemAssemblyResolver Default { get; set; } = new AnyCpuFileSystemAssemblyResolver();

        public FileSystemAssemblyResolver() :
            this(new[] {
                "lib",
            }) {
        }
        public FileSystemAssemblyResolver(IEnumerable<string> probingPaths) {

            if (probingPaths is null)
                throw new ArgumentNullException(nameof(probingPaths));

            foreach (string probingPath in probingPaths)
                ProbingPaths.Add(probingPath);

        }

    }

}
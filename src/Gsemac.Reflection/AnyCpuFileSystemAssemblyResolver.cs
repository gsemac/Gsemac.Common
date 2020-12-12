using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Reflection {

    public class AnyCpuFileSystemAssemblyResolver :
        FileSystemAssemblyResolver {

        // Protected members

        protected override IEnumerable<string> GetProbingPaths() {

            string archPath = Environment.Is64BitProcess ? "x64" : "x86";

            return base.GetProbingPaths().Concat(base.GetProbingPaths()
                .Where(path => !IsArchitectureDependentPath(path))
                .Select(path => Path.Combine(path, archPath)));

        }

        // Private members

        private string GetArchitectureSubdirectory() {

            return Environment.Is64BitProcess ? "x64" : "x86";

        }
        private bool IsArchitectureDependentPath(string path) {

            string archPath = GetArchitectureSubdirectory();

            return path.EndsWith(Path.DirectorySeparatorChar + archPath) ||
                path.EndsWith(Path.AltDirectorySeparatorChar + archPath);

        }

    }

}
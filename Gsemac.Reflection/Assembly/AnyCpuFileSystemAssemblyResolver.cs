using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Reflection.Assembly {

    public class AnyCpuFileSystemAssemblyResolver :
        FileSystemAssemblyResolver {

        // Protected members

        protected override IEnumerable<string> GetProbingPaths() {

            string archPath = Environment.Is64BitProcess ? "x64" : "x86";

            foreach (string probingPath in base.GetProbingPaths()) {

                // Only generate architecture-dependent paths if the current path is not already architecture-dependent.

                if (!probingPath.EndsWith(Path.DirectorySeparatorChar + archPath) && !probingPath.EndsWith(Path.AltDirectorySeparatorChar + archPath))
                    yield return Path.Combine(probingPath, archPath);

                yield return probingPath;

            }

        }

    }

}
using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Assembly {

    public class ArchitectureDependentFileSystemAssemblyResolver :
        FileSystemAssemblyResolver {

        // Protected members

        protected override IEnumerable<string> GetProbingPaths() {

            string archPath = Environment.Is64BitProcess ? "x64" : "x86";

            foreach (string probingPath in ProbingPaths) {

                yield return Path.Combine(probingPath, archPath);
                yield return probingPath;

            }

            if (ProbeCurrentDirectory)
                yield return Path.Combine(new EntryAssemblyInfo().Directory, archPath);

        }

    }

}
using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Assembly {

    public class AnyCpuFileSystemAssemblyResolver :
        FileSystemAssemblyResolver {

        // Protected members

        protected override IEnumerable<string> GetProbingPaths() {

            string archPath = Environment.Is64BitProcess ? "x64" : "x86";

            foreach (string probingPath in base.GetProbingPaths()) {

                yield return Path.Combine(probingPath, archPath);
                yield return probingPath;

            }

        }

    }

}
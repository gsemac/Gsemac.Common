using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gsemac.Core {

    public static class EnvironmentUtilities {

        public static void AddEnvironmentPath(string path) {

            AddEnvironmentPaths(new[] { path });

        }
        public static void AddEnvironmentPaths(IEnumerable<string> paths) {

            // https://stackoverflow.com/a/2864714 (Chris Schmich)

            string[] oldPathValue = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };
            string newPathValue = string.Join(Path.PathSeparator.ToString(), oldPathValue.Concat(paths));

            Environment.SetEnvironmentVariable("PATH", newPathValue);

        }
        public static IEnumerable<string> GetEnvironmentPaths() {

            return Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? Enumerable.Empty<string>();

        }

        public static System.Version GetClrVersion() {

            return Environment.Version;

        }
        public static System.Version GetFrameworkVersion() {

            return new System.Version(FileVersionInfo.GetVersionInfo(typeof(int).Assembly.Location).ProductVersion);

        }

    }

}
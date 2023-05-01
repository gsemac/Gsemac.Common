using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    internal class ArchiveFactoryUtilities {

        // Internal members

        internal static string GetProgramExecutablePath(string directoryName, string fileName) {

            // Scan all program files directories on all drives to find the given directory, and look for the given filename inside.

            IEnumerable<string> driveDirectoryPaths = DriveInfo.GetDrives()
                .Select(info => info.RootDirectory.FullName);

            IEnumerable<string> programDirectoryPaths = new[] {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                @"Program Files",
                @"Program Files (x86)",
            }
            .Distinct()
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(path => Path.Combine(path, directoryName))
            .SelectMany(path => driveDirectoryPaths.Select(drivePath => Path.Combine(drivePath, path)))
            .Distinct();

            IFileSystemAssemblyResolver resolver = new FileSystemAssemblyResolver() {
                AddExtension = false,
            };

            foreach (string probingPath in programDirectoryPaths)
                resolver.ProbingPaths.Add(probingPath);

            return resolver.GetAssemblyPath(fileName);

        }

    }

}
using System;
using System.Diagnostics;

namespace Gsemac.Core.Assembly {

    public class FileSystemAssemblyInfo :
        IAssemblyInfo {

        // Public members

        public string Location { get; }
        public string Filename { get; }
        public string Directory { get; }

        public Version Version { get; }
        public Version FileVersion { get; }

        public FileSystemAssemblyInfo(System.Reflection.Assembly assembly) {

            Location = assembly.Location;
            Filename = System.IO.Path.GetFileName(Location);
            Directory = System.IO.Path.GetDirectoryName(Location);
            Version = assembly.GetName().Version;
            FileVersion = Version.Parse(FileVersionInfo.GetVersionInfo(Location).ProductVersion);

        }

    }

}
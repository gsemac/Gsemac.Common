using System;
using System.Diagnostics;

namespace Gsemac.Assembly {

    public class EntryAssemblyInfo :
        IAssemblyInfo {

        // Public members

        public string Location => GetAssembly().Location;
        public string Filename => System.IO.Path.GetFileName(Location);
        public string Directory => System.IO.Path.GetDirectoryName(Location);
        public Version Version => GetAssembly().GetName().Version;
        public Version FileVersion => Version.Parse(FileVersionInfo.GetVersionInfo(Location).ProductVersion);

        public static string GetLocation() {

            return new EntryAssemblyInfo().Location;

        }
        public static string GetFilename() {

            return new EntryAssemblyInfo().Filename;

        }
        public static string GetDirectory() {

            return new EntryAssemblyInfo().Directory;

        }
        public static Version GetVersion() {

            return new EntryAssemblyInfo().Version;

        }
        public static Version GetFileVersion() {

            return new EntryAssemblyInfo().FileVersion;

        }

        // Private members

        System.Reflection.Assembly GetAssembly() {

            return System.Reflection.Assembly.GetEntryAssembly();

        }

    }

}
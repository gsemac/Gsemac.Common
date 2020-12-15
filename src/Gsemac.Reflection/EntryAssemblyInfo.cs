using System;
using System.Reflection;

namespace Gsemac.Reflection {

    public class EntryAssemblyInfo :
        AssemblyInfo {

        // Public members

        public EntryAssemblyInfo() :
            base(Assembly.GetEntryAssembly()) {
        }

        public static string GetLocation() {

            return new EntryAssemblyInfo().Location;

        }
        public static string GetFilename() {

            return new EntryAssemblyInfo().Filename;

        }
        public static string GetDirectory() {

            return new EntryAssemblyInfo().Directory;

        }
        public static string GetName() {

            return new EntryAssemblyInfo().Name;

        }
        public static Version GetVersion() {

            return new EntryAssemblyInfo().Version;

        }
        public static string GetProductName() {

            return new EntryAssemblyInfo().ProductName;

        }
        public static Version GetProductVersion() {

            return new EntryAssemblyInfo().ProductVersion;

        }



    }

}
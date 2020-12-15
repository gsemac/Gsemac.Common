using System;
using System.Diagnostics;

namespace Gsemac.Reflection {

    public class AssemblyInfo :
        IAssemblyInfo {

        // Public members

        public string Location => assembly.Location;
        public string Filename => System.IO.Path.GetFileName(Location);
        public string Directory => System.IO.Path.GetDirectoryName(Location);
        public string Name => assembly.GetName().Name;
        public Version Version => assembly.GetName().Version;
        public string ProductName => !(fileVersionInfo is null) ? fileVersionInfo.ProductName : Name;
        public Version ProductVersion => !(fileVersionInfo is null) ? Version.Parse(fileVersionInfo.ProductVersion) : Version;

        public AssemblyInfo(System.Reflection.Assembly assembly) {

            this.assembly = assembly;

            if (System.IO.File.Exists(Location))
                fileVersionInfo = FileVersionInfo.GetVersionInfo(Location);

        }

        // Private members

        private readonly System.Reflection.Assembly assembly;
        private readonly FileVersionInfo fileVersionInfo;

    }

}
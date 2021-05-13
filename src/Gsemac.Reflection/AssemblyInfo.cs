using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Gsemac.Reflection {

    public class AssemblyInfo :
        IAssemblyInfo {

        // Public members

        public string Description => GetDescription();
        public string Directory => GetDirectory();
        public string Filename => GetFilename();
        public string Location => GetLocation();
        public string Name => GetName();
        public string ProductName => GetProductName();
        public Version ProductVersion => GetProductVersion();
        public Version Version => GetVersion();

        public static AssemblyInfo CallingAssembly => new AssemblyInfo(Assembly.GetCallingAssembly());
        public static AssemblyInfo EntryAssembly => new AssemblyInfo(Assembly.GetEntryAssembly());
        public static AssemblyInfo ExecutingAssembly => new AssemblyInfo(Assembly.GetExecutingAssembly());

        public AssemblyInfo(Assembly assembly) {

            this.assembly = assembly;
            this.fileVersionInfo = new Lazy<FileVersionInfo>(GetFileVersionInfo);

        }

        // Private members

        private readonly Assembly assembly;
        private readonly Lazy<FileVersionInfo> fileVersionInfo;

        private FileVersionInfo GetFileVersionInfo() {

            if (System.IO.File.Exists(Location))
                return FileVersionInfo.GetVersionInfo(Location);

            return null;

        }

        private string GetDescription() {

            AssemblyDescriptionAttribute descriptionAttribute = Assembly.GetEntryAssembly()
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                .OfType<AssemblyDescriptionAttribute>()
                .FirstOrDefault();

            return descriptionAttribute?.Description ?? string.Empty;

        }
        private string GetDirectory() {

            return System.IO.Path.GetDirectoryName(Location);

        }
        private string GetFilename() {

            return System.IO.Path.GetFileName(Location);

        }
        private string GetLocation() {

            return assembly.Location;

        }
        private string GetName() {

            return assembly.GetName().Name;

        }
        private string GetProductName() {

            return fileVersionInfo.Value?.ProductName ?? Name;

        }
        private Version GetProductVersion() {

            return fileVersionInfo.Value?.ProductVersion is null ? Version : Version.Parse(fileVersionInfo.Value.ProductVersion);

        }
        private Version GetVersion() {

            return assembly.GetName().Version;

        }

    }

}
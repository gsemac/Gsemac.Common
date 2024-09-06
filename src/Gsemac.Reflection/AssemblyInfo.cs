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
        public string FileName => GetFileName();
        public string Location => GetLocation();
        public string Name => GetName();
        public string ProductName => GetProductName();
        public Version ProductVersion => GetProductVersion();
        public Version Version => GetVersion();

        public static AssemblyInfo CallingAssembly => new AssemblyInfo(Assembly.GetCallingAssembly());
        public static AssemblyInfo EntryAssembly => Assembly.GetEntryAssembly() is null ? null : new AssemblyInfo(Assembly.GetEntryAssembly()); // GetEntryAssembly() returns null in some contexts (e.g. from unit testing frameworks)
        public static AssemblyInfo ExecutingAssembly => new AssemblyInfo(Assembly.GetExecutingAssembly());

        public AssemblyInfo(Assembly assembly) {

            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

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
        private string GetFileName() {

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

            string productVersionStr = fileVersionInfo.Value?.ProductVersion;

            if (!string.IsNullOrWhiteSpace(productVersionStr) && Version.TryParse(productVersionStr, out Version parsedProductVersion))
                return parsedProductVersion;

            return Version;

        }
        private Version GetVersion() {

            return assembly.GetName().Version;

        }

    }

}
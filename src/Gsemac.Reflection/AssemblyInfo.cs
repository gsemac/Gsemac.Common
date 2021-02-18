using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Gsemac.Reflection {

    public class AssemblyInfo :
        IAssemblyInfo {

        // Public members

        public string Description => GetDescription();
        public string Directory => System.IO.Path.GetDirectoryName(Location);
        public string Filename => System.IO.Path.GetFileName(Location);
        public string Location => assembly.Location;
        public string Name => assembly.GetName().Name;
        public string ProductName => !(fileVersionInfo is null) ? fileVersionInfo.ProductName : Name;
        public Version ProductVersion => !(fileVersionInfo is null) ? Version.Parse(fileVersionInfo.ProductVersion) : Version;
        public Version Version => assembly.GetName().Version;

        public static AssemblyInfo CallingAssembly => new AssemblyInfo(Assembly.GetCallingAssembly());
        public static AssemblyInfo EntryAssembly => new AssemblyInfo(Assembly.GetEntryAssembly());
        public static AssemblyInfo ExecutingAssembly => new AssemblyInfo(Assembly.GetExecutingAssembly());

        public AssemblyInfo(Assembly assembly) {

            this.assembly = assembly;

            if (System.IO.File.Exists(Location))
                fileVersionInfo = FileVersionInfo.GetVersionInfo(Location);

        }

        // Private members

        private readonly Assembly assembly;
        private readonly FileVersionInfo fileVersionInfo;

        private string GetDescription() {

            AssemblyDescriptionAttribute descriptionAttribute = Assembly.GetEntryAssembly()
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                .OfType<AssemblyDescriptionAttribute>()
                .FirstOrDefault();

            return descriptionAttribute?.Description ?? string.Empty;

        }

    }

}
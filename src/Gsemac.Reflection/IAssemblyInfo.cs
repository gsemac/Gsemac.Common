using System;

namespace Gsemac.Reflection {

    public interface IAssemblyInfo {

        string Location { get; }
        string Filename { get; }
        string Directory { get; }

        string Name { get; }
        Version Version { get; }

        string ProductName { get; }
        Version ProductVersion { get; }

    }

}
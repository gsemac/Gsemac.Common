using System;

namespace Gsemac.Reflection.Assembly {

    public interface IAssemblyInfo {

        string Location { get; }
        string Filename { get; }
        string Directory { get; }

        Version Version { get; }
        Version FileVersion { get; }

    }

}
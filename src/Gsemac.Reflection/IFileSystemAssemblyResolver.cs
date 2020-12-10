using System.Collections.Generic;

namespace Gsemac.Reflection {

    public interface IFileSystemAssemblyResolver :
        IAssemblyResolver {

        bool AddExtension { get; set; }
        ICollection<string> ProbingPaths { get; }
        bool Unsafe { get; set; }

        string GetAssemblyPath(string assemblyName);
        IEnumerable<string> GetAssemblyPaths(string searchPattern = "*.dll");

    }

}
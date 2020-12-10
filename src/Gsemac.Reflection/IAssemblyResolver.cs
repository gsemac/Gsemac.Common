using System;

namespace Gsemac.Reflection {

    public interface IAssemblyResolver {

        System.Reflection.Assembly ResolveAssembly(string assemblyName);
        System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs e);

        bool AssemblyExists(string assemblyName);

    }

}
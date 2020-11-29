using System;

namespace Gsemac.Reflection {

    public interface IAssemblyResolver {

        System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs e);

    }

}
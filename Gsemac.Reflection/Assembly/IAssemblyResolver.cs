using System;

namespace Gsemac.Reflection.Assembly {

    public interface IAssemblyResolver {

        System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs e);

    }

}
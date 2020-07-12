using System;

namespace Gsemac.Assembly {

    public interface IAssemblyResolver {

        System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs e);

    }

}
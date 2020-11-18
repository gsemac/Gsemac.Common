using System;

namespace Gsemac.Core.Assembly {

    public interface IAssemblyResolver {

        System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs e);

    }

}
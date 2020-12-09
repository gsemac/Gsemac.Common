using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Core {

    public static class TypeUtilities {

        public static IEnumerable<Type> GetTypesImplementingInterface<T>() {

            Type interfaceType = typeof(T);

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsAbstract);

            return types;

        }

    }

}
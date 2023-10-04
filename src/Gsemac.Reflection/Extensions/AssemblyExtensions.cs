using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Reflection.Extensions {

    public static class AssemblyExtensions {

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly) {

            IEnumerable<Type> result = Enumerable.Empty<Type>();

            try {

                result = assembly.GetTypes();

            }
            catch (ReflectionTypeLoadException ex) {

                result = ex.Types;

            }

            return result.Where(type => !(type is null));

        }

    }

}
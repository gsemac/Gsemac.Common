using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Reflection {

    public static class TypeUtilities {

        public static IEnumerable<Type> GetTypesImplementingInterface<T>() {

            Type interfaceType = typeof(T);

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsAbstract);

            return types;

        }
        public static Type GetType(string typeName) {

            return AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType(typeName))
                .Where(type => !(type is null))
                .FirstOrDefault();

        }
        public static bool TypeExists(string typeName) {

            return GetType(typeName) != null;

        }

        public static bool TestRequirementAttributes(Type type) {

            IEnumerable<IRequirementAttribute> requirements =
                Attribute.GetCustomAttributes(type).OfType<IRequirementAttribute>();

            return requirements.All(requirement => requirement.IsSatisfied);

        }

    }

}
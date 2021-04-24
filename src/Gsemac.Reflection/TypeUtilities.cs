using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Reflection {

    public static class TypeUtilities {

        public static Type GetType(string typeName) {

            return AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType(typeName))
                .Where(type => !(type is null))
                .FirstOrDefault();

        }
        public static IEnumerable<Type> GetTypes() {

            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetLoadableTypes());

        }
        public static IEnumerable<Type> GetTypesImplementingInterface<T>() {

            return GetTypes().Where(type => type.ImplementsInterface<T>());

        }
        public static bool TypeExists(string typeName) {

            return GetType(typeName) != null;

        }

        public static bool TestRequirementAttributes(Type type) {

            IEnumerable<IRequirementAttribute> requirements =
                Attribute.GetCustomAttributes(type).OfType<IRequirementAttribute>();

            return requirements.All(requirement => requirement.IsSatisfied);

        }

        public static bool TryCast<T>(object obj, out T result) {

            if (TryCast(obj, typeof(T), out object resultObject)) {

                result = (T)resultObject;

                return true;

            }

            result = default;

            return false;

        }
        public static bool TryCast(object obj, Type type, out object result) {

            try {

                if (type.IsEnum)
                    return EnumUtilities.TryParse(obj, type, ignoreCase: true, out result);

                result = Convert.ChangeType(obj, type);

                return true;

            }
            catch (Exception) {

                // I originally had this catch InvalidCastException, but it's also possible for other exceptions to be thrown, such as FormatException when parsing strings.
                // Because this method is never supposed to throw, I've changed it to catch all exceptions.

                result = default;

                return false;

            }

        }

    }

}
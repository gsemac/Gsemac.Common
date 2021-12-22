using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Gsemac.Reflection {

    public static class TypeUtilities {

        // Public members

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

        public static bool TryCast<T>(object obj, out T result) {

            if (TryCast(obj, typeof(T), out object resultObject)) {

                result = (T)resultObject;

                return true;

            }

            result = default;

            return false;

        }
        public static bool TryCast(object obj, Type type, out object result) {

            bool success = true;

            try {

                if (type.IsNullableType() && obj is null) {

                    // If the type is nullable and we have a null object, we can trivially create an instance of the nullable type.
                    // It's important to use null explicitly instead of obj, because CreateInstance will change "null" arguments to "default" for primitive types.

                    result = Activator.CreateInstance(type, null);

                }
                else {

                    // If we have a nullable type and object is not null, we need to cast to the underlying type.
                    // This is because Convert.ChangeType will fail for nullable types.

                    Type newType = type.IsNullableType() ?
                       Nullable.GetUnderlyingType(type) :
                       type;

                    if (newType.IsEnum) {

                        success = EnumUtilities.TryParse(obj, newType, EnumParseOptions.IgnoreCase, out result);

                    }
                    else {

                        result = Convert.ChangeType(obj, newType, CultureInfo.InvariantCulture);

                    }

                    // If we managed to cast the object to the underlying type, then we can easily construct an instance of the nullable type using the casted object.

                    if (type.IsNullableType() && success && result is object)
                        result = Activator.CreateInstance(type, result);

                }

            }
            catch (Exception) {

                // I originally had this catch InvalidCastException, but it's also possible for other exceptions to be thrown, such as FormatException when parsing strings.
                // Because this method is never supposed to throw, I've changed it to catch all exceptions.

                result = default;

                success = false;

            }

            return success;

        }

    }

}
﻿using Gsemac.Reflection.Extensions;
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
        public static IEnumerable<Type> GetTypesImplementingInterface<InterfaceT>() {

            return GetTypes().Where(type => type.ImplementsInterface<InterfaceT>());

        }
        public static IEnumerable<Type> GetTypesWithAttribute<AttributeT>(bool inherit)
            where AttributeT : Attribute {

            return GetTypes().Where(type => HasAttribute<AttributeT>(type, inherit));

        }
        public static bool TypeExists(string typeName) {

            return GetType(typeName) != null;

        }

        public static bool IsDefaultConstructable(Type type) {

            return type.GetConstructor(Type.EmptyTypes) != null;

        }
        public static bool IsConstructableFrom(Type type, IEnumerable<Type> types) {

            return type.GetConstructor(types.ToArray()) is object;

        }
        public static bool IsConstructableFrom(Type type, IEnumerable<object> args) {

            return IsConstructableFrom(type, args.Select(arg => arg.GetType()));

        }

        public static bool ImplementsInterface<InterfaceT>(Type type) {

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Type interfaceType = typeof(InterfaceT);

            return interfaceType.IsAssignableFrom(type) && !type.IsAbstract;

        }
        public static bool HasAttribute<AttributeT>(Type type, bool inherit) {

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return type.GetCustomAttributes(inherit).OfType<AttributeT>().Any();

        }

        public static bool IsNullableType(Type type) {

            return type is object &&
                type.IsGenericType &&
                !type.IsGenericTypeDefinition &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>);

        }
        public static Type GetNullableType(Type type) {

            // https://stackoverflow.com/a/108122 (Alex Lyman)

            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;

        }
        public static bool IsBuiltInType(Type type) {

            // Returns true for built-in types as defined here:
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types

            if (type == typeof(object))
                return true;

            TypeCode typeCode = Type.GetTypeCode(type);

            return typeCode != TypeCode.Object &&
                typeCode != TypeCode.DateTime &&
                typeCode != TypeCode.Empty;

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

                        success = EnumUtilities.TryParse(obj, newType, new EnumParseOptions() { IgnoreCase = true, }, out result);

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
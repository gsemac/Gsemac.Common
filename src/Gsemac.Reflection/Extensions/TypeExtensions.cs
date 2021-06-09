using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Reflection.Extensions {

    public static class TypeExtensions {

        public static bool IsDefaultConstructable(this Type type) {

            return type.GetConstructor(Type.EmptyTypes) != null;

        }
        public static bool IsConstructableFrom(this Type type, IEnumerable<Type> types) {

            return type.GetConstructor(types.ToArray()) is object;

        }
        public static bool IsConstructableFrom(this Type type, IEnumerable<object> args) {

            return IsConstructableFrom(type, args.Select(arg => arg.GetType()));

        }

        public static bool ImplementsInterface<T>(this Type type) {

            Type interfaceType = typeof(T);

            return interfaceType.IsAssignableFrom(type) && !type.IsAbstract;

        }

        public static bool IsNullableType(this Type type) {

            return type != null &&
                type.IsGenericType &&
                !type.IsGenericTypeDefinition &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>);

        }
        public static Type GetNullableType(this Type type) {

            // https://stackoverflow.com/a/108122 (Alex Lyman)

            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;

        }

        public static bool IsBuiltInType(this Type type) {

            // Returns true for built-in types as defined here:
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types

            if (type == typeof(object))
                return true;

            TypeCode typeCode = Type.GetTypeCode(type);

            return typeCode != TypeCode.Object &&
                typeCode != TypeCode.DateTime &&
                typeCode != TypeCode.Empty;

        }

    }

}
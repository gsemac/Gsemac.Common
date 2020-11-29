using System;

namespace Gsemac.Core.Extensions {

    public static class TypeExtensions {

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

    }

}
using System;
using System.Collections.Generic;

namespace Gsemac.Reflection.Extensions {

    public static class TypeExtensions {

        public static bool IsDefaultConstructable(this Type type) {

            return TypeUtilities.IsDefaultConstructable(type);

        }
        public static bool IsConstructableFrom(this Type type, IEnumerable<Type> types) {

            return TypeUtilities.IsConstructableFrom(type, types);

        }
        public static bool IsConstructableFrom(this Type type, IEnumerable<object> args) {

            return TypeUtilities.IsConstructableFrom(type, args);

        }

        public static bool ImplementsInterface<InterfaceT>(this Type type) {

            return TypeUtilities.ImplementsInterface<InterfaceT>(type);

        }
        public static bool HasAttribute<AttributeT>(this Type type, bool inherit) {

            return TypeUtilities.HasAttribute<AttributeT>(type, inherit);

        }

        public static bool IsNullableType(this Type type) {

            return TypeUtilities.IsNullableType(type);

        }
        public static Type GetNullableType(this Type type) {

            return TypeUtilities.GetNullableType(type);

        }

        public static bool IsBuiltInType(this Type type) {

            return TypeUtilities.IsBuiltInType(type);

        }

    }

}
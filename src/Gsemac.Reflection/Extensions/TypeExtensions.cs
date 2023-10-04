using System;
using System.Collections.Generic;

namespace Gsemac.Reflection.Extensions {

    public static class TypeExtensions {

        // Public members

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

        public static bool IsNullable(this Type type) {

            return TypeUtilities.IsNullable(type);

        }
        public static Type GetNullable(this Type type) {

            return TypeUtilities.GetNullable(type);

        }
        public static bool IsBuiltIn(this Type type) {

            return TypeUtilities.IsBuiltIn(type);

        }
        public static bool IsNumeric(this Type type) {

            return TypeUtilities.IsNumeric(type);

        }

    }

}
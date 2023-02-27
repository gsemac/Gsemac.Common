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

            return type.GetConstructor(Type.EmptyTypes) is object;

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

        public static bool IsNullable(Type type) {

            return type is object &&
                type.IsGenericType &&
                !type.IsGenericTypeDefinition &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>);

        }
        public static Type GetNullable(Type type) {

            // https://stackoverflow.com/a/108122 (Alex Lyman)

            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;

        }
        public static bool IsBuiltIn(Type type) {

            // Returns true for built-in types as defined here:
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types

            if (type == typeof(object))
                return true;

            TypeCode typeCode = Type.GetTypeCode(type);

            return typeCode != TypeCode.Object &&
                typeCode != TypeCode.DateTime &&
                typeCode != TypeCode.Empty;

        }
        public static bool IsNumeric(Type type) {

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            // https://stackoverflow.com/a/1750024/5383169 (Philip Wallace)

            switch (Type.GetTypeCode(type)) {

                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                default:
                    return false;

            }

        }

        public static bool TryCast<T>(object obj, out T result) {

            return TryCast(obj, CastOptions.Default, out result);

        }
        public static bool TryCast<T>(object obj, ICastOptions options, out T result) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (TryCast(obj, typeof(T), options, out object resultObject)) {

                result = (T)resultObject;

                return true;

            }

            result = default;

            return false;

        }
        public static bool TryCast(object obj, Type type, out object result) {

            return TryCast(obj, type, CastOptions.Default, out result);

        }
        public static bool TryCast(object obj, Type type, ICastOptions options, out object result) {

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            result = default;

            bool success = true;

            bool isNullableType = type.IsNullable();

            try {

                if (isNullableType && obj is null) {

                    // If the type is nullable and we have a null object, we can trivially create an instance of the nullable type.
                    // It's important to use null explicitly instead of obj, because CreateInstance will change "null" arguments to "default" for primitive types.

                    result = Activator.CreateInstance(type, null);

                }
                else {

                    // If we have a nullable type and object is not null, we need to cast to the underlying type.
                    // This is because Convert.ChangeType will fail for nullable types.

                    Type newType = isNullableType ?
                       Nullable.GetUnderlyingType(type) :
                       type;

                    if (obj is object && newType.IsAssignableFrom(obj.GetType())) {

                        // The object can be casted directly to the target type.

                        result = obj;

                    }
                    else if (newType.IsEnum) {

                        success = EnumUtilities.TryParse(obj, newType, new EnumParseOptions() { IgnoreCase = options.IgnoreCase, }, out result);

                    }
                    else {

                        try {

                            result = Convert.ChangeType(obj, newType, CultureInfo.InvariantCulture);

                        }
                        catch (Exception) {

                            // I originally had this catch InvalidCastException, but it's also possible for other exceptions to be thrown, such as FormatException when parsing strings.
                            // Because this method is never supposed to throw, I've changed it to catch all exceptions.

                            if (obj is object && newType.Equals(typeof(string))) {

                                // Convert any object to a string by calling the ToString method.

                                result = obj.ToString();

                            }
                            else if (options.EnableConstructorInitialization) {

                                // Attempt to create an instance of the object using constructor initialization.

                                result = Activator.CreateInstance(newType, new[] { obj });

                            }
                            else {

                                throw;

                            }

                        }

                    }

                    // If we managed to cast the object to the underlying type, then we can easily construct an instance of the nullable type using the casted object.

                    if (type.IsNullable() && success && result is object)
                        result = Activator.CreateInstance(type, result);

                }

            }
            catch (Exception) {

                success = false;

            }

            return success;

        }

    }

}
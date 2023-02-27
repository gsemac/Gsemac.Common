using System;
using System.Globalization;

namespace Gsemac.Reflection {

    public static class EnumUtilities {

        // Public members

        public static object Parse(object obj, Type enumType) {

            return Parse(obj, enumType, EnumParseOptions.Default);

        }
        public static object Parse(object obj, Type enumType, EnumParseOptions options) {

            if (!enumType.IsEnum)
                throw new ArgumentException(Properties.ExceptionMessages.TypeMustBeAnEnum, nameof(enumType));

            if (obj is string inputAsString) {

                // Attempt to parse the enum from a string, which can be either an integer or the name of one of the enumeration values.

                if (int.TryParse(inputAsString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int inputStringAsInt))
                    return Enum.ToObject(enumType, inputStringAsInt);
                else
                    return Enum.Parse(enumType, inputAsString, options.IgnoreCase);

            }
            else {

                // Attempt to parse the enum from an integer.
                // This will through if the given object is not an integral type.

                return Enum.ToObject(enumType, obj);

            }

        }
        public static bool TryParse(object obj, Type enumType, out object result) {

            return TryParse(obj, enumType, EnumParseOptions.Default, out result);

        }
        public static bool TryParse(object obj, Type enumType, EnumParseOptions options, out object result) {

            if (!enumType.IsEnum)
                throw new ArgumentException(Properties.ExceptionMessages.TypeMustBeAnEnum, nameof(enumType));

            result = default;

            try {

                result = Parse(obj, enumType, options);

                return true;

            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception) {

                return false;

            }
#pragma warning restore CA1031 // Do not catch general exception types

        }

        public static EnumT Parse<EnumT>(object obj) where EnumT : Enum {

            return Parse<EnumT>(obj, EnumParseOptions.Default);

        }
        public static EnumT Parse<EnumT>(object obj, EnumParseOptions options) where EnumT : Enum {

            return (EnumT)Parse(obj, typeof(EnumT), options);

        }
        public static bool TryParse<EnumT>(object obj, out EnumT result) where EnumT : Enum {

            return TryParse(obj, EnumParseOptions.Default, out result);

        }
        public static bool TryParse<EnumT>(object obj, EnumParseOptions options, out EnumT result) where EnumT : Enum {

            result = default;

            if (TryParse(obj, typeof(EnumT), options, out object tryParseResult)) {

                result = (EnumT)tryParseResult;

                return true;

            }

            return false;

        }

    }

}
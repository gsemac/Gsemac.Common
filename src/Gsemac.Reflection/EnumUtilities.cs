using System;
using System.Globalization;

namespace Gsemac.Reflection {

    public static class EnumUtilities {


        public static bool TryParse(object obj, Type enumType, out object result) {

            return TryParse(obj, enumType, ignoreCase: false, out result);

        }
        public static bool TryParse(object obj, Type enumType, bool ignoreCase, out object result) {

            result = default;

            if (!enumType.IsEnum)
                return false;

            try {

                if (obj is string inputAsString) {

                    if (int.TryParse(inputAsString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int inputStringAsInt))
                        result = Enum.ToObject(enumType, inputStringAsInt);
                    else
                        result = Enum.Parse(enumType, inputAsString, ignoreCase);

                }
                else {

                    // For this to work, "obj" must be an integral type.

                    result = Enum.ToObject(enumType, obj);

                }

                return true;

            }
            catch (Exception) {

                return false;

            }

        }

    }

}
using System;

namespace Gsemac.Reflection {

    public static class ObjectUtilities {

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
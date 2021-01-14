using System;

namespace Gsemac.Reflection.Extensions {

    public static class ObjectExtensions {

        public static IObjectPropertyDictionary ToDictionary(this object obj, ObjectPropertyDictionaryOptions options = ObjectPropertyDictionaryOptions.Default) {

            return new ObjectPropertyDictionary(obj, options);

        }

        public static bool TryCast<T>(this object obj, out T result) {

            if (TryCast(obj, typeof(T), out object resultObject)) {

                result = (T)resultObject;

                return true;

            }

            result = default;

            return false;

        }
        public static bool TryCast(this object obj, Type type, out object result) {

            try {

                result = Convert.ChangeType(obj, type);

                return true;

            }
            catch (InvalidCastException) {

                result = default;

                return false;

            }

        }

    }

}
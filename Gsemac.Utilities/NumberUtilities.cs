using System;

namespace Gsemac.Utilities {

    public static class NumberUtilities {

        public static bool TryParse(string s, int radix, out int result) {

            try {

                result = Convert.ToInt32(s, radix);

                return true;

            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception) {

                result = default;

                return false;

            }
#pragma warning restore CA1031 // Do not catch general exception types

        }

    }

}
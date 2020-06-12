using System;

namespace Gsemac.Utilities {

    public static class StringUtilities {

        public static string After(string input, string substring) {

            return After(input, substring, StringComparison.CurrentCulture);

        }
        public static string After(string input, string substring, StringComparison comparisonType) {

            int index = input.IndexOf(substring, comparisonType);

            if (index >= 0) {

                index += substring.Length;

                return input.Substring(index, input.Length - index);

            }
            else
                return input;

        }

    }

}
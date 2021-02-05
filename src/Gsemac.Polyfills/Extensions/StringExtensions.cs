using System;

namespace Gsemac.Polyfills.Extensions {

    public static class StringExtensions {

        public static bool Contains(this string str, char value, StringComparison comparisonType) {

            return str.Contains(value.ToString(), comparisonType);

        }
        public static bool Contains(this string str, string value, StringComparison comparisonType) {

            return str.IndexOf(value, comparisonType) >= 0;

        }

    }

}
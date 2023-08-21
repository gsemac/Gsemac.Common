using System;

namespace Gsemac.Polyfills.System {

    public static class StringExtensions {

        // Public members

        public static bool Contains(this string str, char value, StringComparison comparisonType) {

            return str.Contains(value.ToString(), comparisonType);

        } // .NET Core 2.1 and later
        public static bool Contains(this string str, string value, StringComparison comparisonType) {

            return str.IndexOf(value, comparisonType) >= 0;

        } // .NET Core 2.1 and later

    }

}
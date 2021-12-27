using System.Collections.Generic;

namespace Gsemac.Text.Extensions {

    public static class StringSplitExtensions {

        // Public members

        public static IEnumerable<string> Split(this string value, char separator, int count, StringSplitOptions options = StringSplitOptions.None) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, string[] separator, int count, StringSplitOptions options = StringSplitOptions.None) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, char[] separators, int count, StringSplitOptions options = StringSplitOptions.None) {

            return StringUtilities.Split(value, separators, count, options);

        }
        public static IEnumerable<string> Split(this string value, string[] separators, StringSplitOptions options = StringSplitOptions.None) {

            return StringUtilities.Split(value, separators, options);

        }
        public static IEnumerable<string> Split(this string value, string separator, int count, StringSplitOptions options = StringSplitOptions.None) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, char[] separators, StringSplitOptions options = StringSplitOptions.None) {

            return StringUtilities.Split(value, separators, options);

        }
        public static IEnumerable<string> Split(this string value, char separator, StringSplitOptions options = StringSplitOptions.None) {

            return StringUtilities.Split(value, separator, options);

        }
        public static IEnumerable<string> Split(this string value, string separator, StringSplitOptions options = StringSplitOptions.None) {

            return StringUtilities.Split(value, separator, options);

        }

    }

}
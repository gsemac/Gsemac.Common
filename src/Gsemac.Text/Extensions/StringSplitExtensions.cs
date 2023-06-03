using System.Collections.Generic;

namespace Gsemac.Text.Extensions {

    public static class StringSplitExtensions {

        // Public members

        public static IEnumerable<string> Split(this string value, char separator, int count, StringSplitOptionsEx options = StringSplitOptionsEx.None) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, string[] separator, int count, StringSplitOptionsEx options = StringSplitOptionsEx.None) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, char[] separators, int count, StringSplitOptionsEx options = StringSplitOptionsEx.None) {

            return StringUtilities.Split(value, separators, count, options);

        }
        public static IEnumerable<string> Split(this string value, string[] separators, StringSplitOptionsEx options = StringSplitOptionsEx.None) {

            return StringUtilities.Split(value, separators, options);

        }
        public static IEnumerable<string> Split(this string value, string separator, int count, StringSplitOptionsEx options = StringSplitOptionsEx.None) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, char[] separators, StringSplitOptionsEx options = StringSplitOptionsEx.None) {

            return StringUtilities.Split(value, separators, options);

        }
        public static IEnumerable<string> Split(this string value, char separator, StringSplitOptionsEx options = StringSplitOptionsEx.None) {

            return StringUtilities.Split(value, separator, options);

        }
        public static IEnumerable<string> Split(this string value, string separator, StringSplitOptionsEx options = StringSplitOptionsEx.None) {

            return StringUtilities.Split(value, separator, options);

        }

    }

}
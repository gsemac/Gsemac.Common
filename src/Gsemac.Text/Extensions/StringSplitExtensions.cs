using System.Collections.Generic;

namespace Gsemac.Text.Extensions {

    public static class StringSplitExtensions {

        // Public members

        public static IEnumerable<string> Split(this string value, char separator, int count) {

            return StringUtilities.Split(value, separator, count, StringSplitOptionsEx.Default);

        }
        public static IEnumerable<string> Split(this string value, string[] separator, int count) {

            return StringUtilities.Split(value, separator, count, StringSplitOptionsEx.Default);

        }
        public static IEnumerable<string> Split(this string value, char[] separators, int count) {

            return StringUtilities.Split(value, separators, count, StringSplitOptionsEx.Default);

        }
        public static IEnumerable<string> Split(this string value, string[] separators) {

            return StringUtilities.Split(value, separators, StringSplitOptionsEx.Default);

        }
        public static IEnumerable<string> Split(this string value, string separator, int count) {

            return StringUtilities.Split(value, separator, count, StringSplitOptionsEx.Default);

        }
        public static IEnumerable<string> Split(this string value, char[] separators) {

            return StringUtilities.Split(value, separators, StringSplitOptionsEx.Default);

        }
        public static IEnumerable<string> Split(this string value, char separator) {

            return StringUtilities.Split(value, separator, StringSplitOptionsEx.Default);

        }
        public static IEnumerable<string> Split(this string value, string separator) {

            return StringUtilities.Split(value, separator, StringSplitOptionsEx.Default);

        }
        public static IEnumerable<string> Split(this string value, char separator, int count, IStringSplitOptionsEx options) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, string[] separator, int count, IStringSplitOptionsEx options) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, char[] separators, int count, IStringSplitOptionsEx options) {

            return StringUtilities.Split(value, separators, count, options);

        }
        public static IEnumerable<string> Split(this string value, string[] separators, IStringSplitOptionsEx options) {

            return StringUtilities.Split(value, separators, options);

        }
        public static IEnumerable<string> Split(this string value, string separator, int count, IStringSplitOptionsEx options) {

            return StringUtilities.Split(value, separator, count, options);

        }
        public static IEnumerable<string> Split(this string value, char[] separators, IStringSplitOptionsEx options) {

            return StringUtilities.Split(value, separators, options);

        }
        public static IEnumerable<string> Split(this string value, char separator, IStringSplitOptionsEx options) {

            return StringUtilities.Split(value, separator, options);

        }
        public static IEnumerable<string> Split(this string value, string separator, IStringSplitOptionsEx options) {

            return StringUtilities.Split(value, separator, options);

        }

    }

}
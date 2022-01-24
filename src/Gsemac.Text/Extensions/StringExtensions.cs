using System;

namespace Gsemac.Text.Extensions {

    public static class StringExtensions {

        public static string ToCase(this string input, StringCasing casing) {

            return CaseConverter.ToCase(input, casing);

        }
        public static string ToCase(this string input, StringCasing casing, CasingOptions options) {

            return CaseConverter.ToCase(input, casing, options);

        }
        public static string ToProper(this string input) {

            return CaseConverter.ToProper(input);

        }
        public static string ToProper(this string input, CasingOptions options) {

            return CaseConverter.ToProper(input, options);

        }
        public static string ToSentence(this string input) {

            return CaseConverter.ToSentence(input);

        }
        public static string ToSentence(this string input, CasingOptions options) {

            return CaseConverter.ToSentence(input, options);

        }

        public static string After(this string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.After(input, substring, comparisonType);

        }
        public static string AfterLast(this string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.AfterLast(input, substring, comparisonType);

        }
        public static string Before(this string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.Before(input, substring, comparisonType);

        }
        public static string BeforeLast(this string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.BeforeLast(input, substring, comparisonType);

        }
        public static string Between(this string input, string leftSubstring, string rightSubstring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.Between(input, leftSubstring, rightSubstring, comparisonType);

        }
        public static string BetweenLast(this string input, string leftSubstring, string rightSubstring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.BetweenLast(input, leftSubstring, rightSubstring, comparisonType);

        }

        public static string Reverse(this string input) {

            return StringUtilities.Reverse(input);

        }

        public static string ReplaceFirst(this string input, string oldValue, string newValue, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.ReplaceFirst(input, oldValue, newValue, comparisonType);

        }
        public static string ReplaceLast(this string input, string oldValue, string newValue, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.ReplaceLast(input, oldValue, newValue, comparisonType);

        }

        public static string TrimStart(this string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.TrimStart(input, substring, comparisonType);

        }
        public static string TrimEnd(this string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.TrimEnd(input, substring, comparisonType);

        }
        public static string Trim(this string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return StringUtilities.Trim(input, substring, comparisonType);

        }
        public static string TrimOrDefault(this string input) {

            return StringUtilities.TrimOrDefault(input);

        }

    }

}
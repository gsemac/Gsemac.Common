using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static IEnumerable<string> SplitAfter(string input, params char[] delimiters) {

            string pattern = "([" + Regex.Escape(string.Join("", delimiters)) + "])";
            string[] items = Regex.Split(input, pattern);

            for (int i = 0; i < items.Count(); i += 2) {

                string item = items[i];

                if (i + 1 < items.Count())
                    item += items[i + 1];

                yield return item;

            }

        }

        public static string NormalizeSpace(string input, NormalizeSpaceOptions options = NormalizeSpaceOptions.Default) {

            string pattern = options.HasFlag(NormalizeSpaceOptions.PreserveLineBreaks) ?
                @"[^\S\r\n]" :
                @"\s+";

            return Regex.Replace(input, pattern, " ");

        }

    }

}
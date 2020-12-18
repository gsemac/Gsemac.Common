using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Gsemac.Collections {

    public class NaturalSortComparer :
        IComparer<string>,
        IComparer<FileInfo> {

        // Public members

        public NaturalSortComparer() :
            this(CultureInfo.CurrentCulture) {
        }
        public NaturalSortComparer(CultureInfo cultureInfo) {

            this.cultureInfo = cultureInfo;

        }

        public int Compare(string x, string y) {

            // Each "part" of the string is compared separately, so need to break it up into pieces.

            IEnumerator<string> partsX = GetParts(x).GetEnumerator();
            IEnumerator<string> partsY = GetParts(y).GetEnumerator();

            int compareResult;

            while (partsX.MoveNext() && partsY.MoveNext()) {

                // We only need one part to differ to decide how to order the strings.

                if ((compareResult = CompareParts(partsX.Current, partsY.Current)) != 0)
                    return compareResult;

            }

            // If we get here, the strings are either equal, or one of them is null/empty.

            return cultureInfo.CompareInfo.Compare(x, y, CompareOptions.IgnoreCase);

        }
        public int Compare(FileInfo x, FileInfo y) {

            return Compare(x.FullName, y.FullName);

        }

        // Private members

        private readonly CultureInfo cultureInfo;

        private bool IsCharNumeric(char input) {

            // Numeric sequences include all leading hyphens, which are ignored when comparing the numbers.

            return input == '-' || char.IsDigit(input);

        }
        private bool IsCharDelimiter(char input) {

            // By treating the period as a delimiter, we can sort filenames as expected.
            // http://archives.miloush.net/michkap/archive/2006/10/01/778990.html

            return input == '.';

        }
        private bool ConsumeNumericSequence(string input, ref int startIndex, ref int endIndex) {

            while (endIndex < input.Length && IsCharNumeric(input[endIndex]))
                ++endIndex;

            return startIndex != endIndex;

        }
        private bool ConsumeAlphabeticSequence(string input, ref int startIndex, ref int endIndex) {

            while (endIndex < input.Length && !IsCharDelimiter(input[endIndex]) && !IsCharNumeric(input[endIndex]))
                ++endIndex;

            return startIndex != endIndex;

        }

        private IEnumerable<string> GetParts(string input) {

            int startIndex = 0;
            int endIndex = 0;

            if (!(input is null)) {

                while (startIndex < input.Length) {

                    if (ConsumeNumericSequence(input, ref startIndex, ref endIndex) || ConsumeAlphabeticSequence(input, ref startIndex, ref endIndex)) {

                        yield return input.Substring(startIndex, endIndex - startIndex);

                        startIndex = endIndex;

                    }
                    else {

                        break;

                    }

                }

            }

        }
        private int CompareParts(string x, string y) {

            if (int.TryParse(x.TrimStart('-'), NumberStyles.Integer, cultureInfo, out int intX) && int.TryParse(y.TrimStart('-'), NumberStyles.Integer, cultureInfo, out int intY)) {

                int compareResult;

                if ((compareResult = intX.CompareTo(intY)) != 0)
                    return compareResult;

                // If two numbers compare equal but have a diffing number of leading 0s, the one with the most 0s comes first.

                if ((compareResult = y.TrimStart('-').Length.CompareTo(x.TrimStart('-').Length)) != 0)
                    return compareResult;

                // If the two numbers compare equal, and have the same number of leading zeros, a number with leading hyphens comes first.
                // If there is more than one hyphen, the number with the least number of hyphens comes first.

                return x.Length.CompareTo(y.Length);

            }
            else {

                // Compare as strings.
                // Comparisons are not case-sensitive.

                return cultureInfo.CompareInfo.Compare(x, y, CompareOptions.IgnoreCase);

            }

        }

    }

}
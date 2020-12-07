using System;
using System.Text.RegularExpressions;

namespace Gsemac.Text {

    public static class RegexUtilities {

        public static bool IsRegexValid(string pattern) {

            if (string.IsNullOrEmpty(pattern))
                return false;

            try {

                Regex regex = new Regex(pattern);

                return true;

            }
            catch (ArgumentException) {

                return false;

            }

        }

    }

}
using System;
using System.Text.RegularExpressions;

namespace Gsemac.Core {

    public static class Version {

        public static bool TryParse(string input, out IVersion result) {

            result = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Strip any extraneous text from the version string.
            // Allow inputs like "version 1.0" and "v1.0".

            input = Regex.Replace(input, @"^v(?:ersion)?\s*", string.Empty, RegexOptions.IgnoreCase);

            if (MSVersion.TryParse(input, out MSVersion msVersion))
                result = msVersion;
            else if (SemVersion.TryParse(input, out SemVersion semVersion))
                result = semVersion;

            return result != null;

        }
        public static IVersion Parse(string input) {

            if (input is null)
                throw new ArgumentNullException(nameof(input));

            if (!TryParse(input, out IVersion result))
                throw new FormatException("The version string was not in the correct format.");

            return result;

        }

    }

}
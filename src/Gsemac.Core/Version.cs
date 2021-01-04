using System;

namespace Gsemac.Core {

    public static class Version {

        public static bool TryParse(string input, out IVersion result) {

            result = null;

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
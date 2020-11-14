using Gsemac.Core;

namespace Gsemac.Utilities.Extensions {

    public static class StringExtensions {

        public static string ToProper(this string input, CasingOptions options = CasingOptions.Default) {

            return StringUtilities.ToProperCase(input, options);

        }

    }

}
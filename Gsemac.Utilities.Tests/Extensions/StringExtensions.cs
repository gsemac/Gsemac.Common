namespace Gsemac.Utilities.Tests.Extensions {

    public static class StringExtensions {

        public static string ToProper(this string input, ProperCaseOptions options = ProperCaseOptions.Default) {

            return StringUtilities.ToProperCase(input, options);

        }

    }

}
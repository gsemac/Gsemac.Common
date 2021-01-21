namespace Gsemac.Text.Extensions {

    public static class StringExtensions {

        public static string ToProper(this string input, CasingOptions options = CasingOptions.Default) {

            return CaseConverter.ToProperCase(input, options);

        }

    }

}
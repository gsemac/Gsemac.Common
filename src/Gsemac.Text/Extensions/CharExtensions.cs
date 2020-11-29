namespace Gsemac.Text.Extensions {

    public static class CharExtensions {

        public static bool IsNewLine(this char c) {

            return c == '\r' || c == '\n';

        }

    }

}
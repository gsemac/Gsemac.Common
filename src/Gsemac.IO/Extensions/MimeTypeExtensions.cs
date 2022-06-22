using Gsemac.Text.PatternMatching;

namespace Gsemac.IO.Extensions {

    public static class MimeTypeExtensions {

        // Public members

        public static bool IsMatch(this IMimeType mimeType, IMimeType other) {

            return new WildcardPattern(mimeType.ToString()).IsMatch(other.ToString());

        }

    }

}
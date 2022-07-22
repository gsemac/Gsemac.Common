using System;
using System.Reflection;

namespace Gsemac.Net {

    public static class UriUtilities {

        // Public members

        public static bool SetFilePathCanonicalizationEnabled(bool enabled) {

            // .NET 4.0's Uri implementation doesn't work correctly for URIs that contain trailing periods (fixed in .NET 4.5).
            // https://stackoverflow.com/questions/856885/httpwebrequest-to-url-with-dot-at-the-end
            // The solution below is the one given in the official bug report, as given here: https://stackoverflow.com/a/2285321 (Jon Davis)

            MethodInfo getSyntax = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo flagsField = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);

            if (getSyntax is null || flagsField is null)
                return false;

            foreach (string scheme in new[] { "http", "https" }) {

                UriParser parser = (UriParser)getSyntax.Invoke(null, new object[] { scheme });

                if (parser is null)
                    continue;

                UriSyntaxFlags flags = (UriSyntaxFlags)flagsField.GetValue(parser);

                if (enabled)
                    flags |= UriSyntaxFlags.CanonicalizeAsFilePath;
                else
                    flags &= ~UriSyntaxFlags.CanonicalizeAsFilePath;

                flagsField.SetValue(parser, flags);

            }

            return true;

        }

        public static bool SetPathCanonicalizationEnabled(Uri uri, bool enabled) {

            // .NET 4.0's Uri implementation doesn't work correctly for URIs that contain escaped slashes (fixed in .NET 4.5).
            // It attempts to unescape escaped slashes, which can cause problems for endpoints that expect them to be escaped:
            // https://docs.microsoft.com/en-us/dotnet/api/system.uri?view=netcore-3.1#remarks

            // See the following discussions of the issue:
            // https://stackoverflow.com/q/59846369/5383169
            // https://stackoverflow.com/q/781205/5383169

            // The following solution is adapted from the one given here: https://stackoverflow.com/a/784937/5383169 (Rasmus Faber)

            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            // We need to access the "PathAndQuery" property before setting the flag.

            string _ = uri.PathAndQuery;

            FieldInfo flagsField = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);

            if (flagsField is null)
                return false;

            UriFlags flags = (UriFlags)(ulong)flagsField.GetValue(uri);

            if (enabled)
                flags |= UriFlags.PathNotCanonical | UriFlags.QueryNotCanonical;
            else
                flags &= ~(UriFlags.PathNotCanonical | UriFlags.QueryNotCanonical);

            flagsField.SetValue(uri, (ulong)flags);

            return true;

        }

        // Private members

        [Flags]
        private enum UriSyntaxFlags {
            CanonicalizeAsFilePath = 0x1000000,
        }

        // https://referencesource.microsoft.com/#System/net/System/URI.cs,a5f3168ce0003968,references

        [Flags]
        private enum UriFlags {
            PathNotCanonical = 0x10,
            QueryNotCanonical = 0x20,
        }

    }

}
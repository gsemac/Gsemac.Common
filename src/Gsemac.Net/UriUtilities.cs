using System;
using System.Reflection;

namespace Gsemac.Net {

    public static class UriUtilities {

        // Public members

        public static void SetCanonicalizeAsFilePath(bool enabled) {

            // .NET 4.0's Uri class implementation doesn't work correctly for URIs that contain trailing periods (fixed in .NET 4.5).
            // https://stackoverflow.com/questions/856885/httpwebrequest-to-url-with-dot-at-the-end
            // The solution below is the one given in the official bug report, as given here: https://stackoverflow.com/a/2285321 (Jon Davis)

            MethodInfo getSyntax = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo flagsField = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);

            if (getSyntax is object && flagsField is object) {

                foreach (string scheme in new[] { "http", "https" }) {

                    UriParser parser = (UriParser)getSyntax.Invoke(null, new object[] { scheme });

                    if (parser is object) {

                        UriSyntaxFlags flagsValue = (UriSyntaxFlags)flagsField.GetValue(parser);

                        if (enabled) {

                            // Set the CanonicalizeAsFilePath flag

                            if (!flagsValue.HasFlag(UriSyntaxFlags.CanonicalizeAsFilePath))
                                flagsField.SetValue(parser, flagsValue | UriSyntaxFlags.CanonicalizeAsFilePath);

                        }
                        else {

                            // Clear the CanonicalizeAsFilePath flag

                            if (flagsValue.HasFlag(UriSyntaxFlags.CanonicalizeAsFilePath))
                                flagsField.SetValue(parser, flagsValue & ~UriSyntaxFlags.CanonicalizeAsFilePath);

                        }

                    }

                }

            }

        }

        // Private members

        [Flags]
        private enum UriSyntaxFlags {
            CanonicalizeAsFilePath = 0x1000000,
        }

    }

}
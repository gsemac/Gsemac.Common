using System;

namespace Gsemac.IO.Logging.Extensions {

    public static class LogHeaderCollectionExtensions {

        // Public members

        public static void Add(this ILogHeaderCollection headers, LogHeaderKey key, Func<string> getter) {

            headers.Add(LogHeaderKeyToString(key), getter);

        }
        public static void Add(this ILogHeaderCollection headers, LogHeaderKey key, string value) {

            headers.Add(key, () => value);

        }
        public static bool Remove(this ILogHeaderCollection headers, LogHeaderKey key) {

            return headers.Remove(LogHeaderKeyToString(key));

        }
        public static bool ContainsKey(this ILogHeaderCollection headers, LogHeaderKey key) {

            return headers.ContainsKey(LogHeaderKeyToString(key));

        }
        public static bool TryGetValue(this ILogHeaderCollection headers, LogHeaderKey key, out string value) {

            return headers.TryGetValue(LogHeaderKeyToString(key), out value);

        }

        // Private members

        private static string LogHeaderKeyToString(LogHeaderKey key) {

            switch (key) {

                case LogHeaderKey.ProductVersion:
                    return "Product Version";

                case LogHeaderKey.ClrVersion:
                    return "CLR Version";

                case LogHeaderKey.FrameworkVersion:
                    return "Framework Version";

                case LogHeaderKey.OSVersion:
                    return "OS Version";

                case LogHeaderKey.Locale:
                    return "Locale";

                case LogHeaderKey.Path:
                    return "Path";

                case LogHeaderKey.WorkingDirectory:
                    return "Working Directory";

                case LogHeaderKey.Timestamp:
                    return "Timestamp";

                default:
                    throw new ArgumentOutOfRangeException(nameof(key));

            }

        }

    }

}
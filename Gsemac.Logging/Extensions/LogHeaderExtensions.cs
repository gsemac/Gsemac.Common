using System;

namespace Gsemac.Logging.Extensions {

    public static class LogHeaderExtensions {

        // Public members

        public static void Add(this ILogHeader logHeader, LogHeaderKey key, Func<string> getter) {

            logHeader.Add(LogHeaderKeyToString(key), getter);

        }
        public static void Add(this ILogHeader logHeader, LogHeaderKey key, string value) {

            logHeader.Add(key, () => value);

        }
        public static bool Remove(this ILogHeader logHeader, LogHeaderKey key) {

            return logHeader.Remove(LogHeaderKeyToString(key));

        }
        public static bool ContainsKey(this ILogHeader logHeader, LogHeaderKey key) {

            return logHeader.ContainsKey(LogHeaderKeyToString(key));

        }
        public static bool TryGetValue(this ILogHeader logHeader, LogHeaderKey key, out string value) {

            return logHeader.TryGetValue(LogHeaderKeyToString(key), out value);

        }

        // Private members

        private static string LogHeaderKeyToString(LogHeaderKey key) {

            switch (key) {

                case LogHeaderKey.ProductVersion:
                    return "Product Version";

                case LogHeaderKey.ClrVersion:
                    return "CLR Version";

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
using System;

namespace Gsemac.Core {

    public static class DateUtilities {

        // Public members

        public static DateTimeOffset UnixEpoch => new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

        public static long ToUnixTimeSeconds(DateTimeOffset input) {

            return (long)input.ToUniversalTime().Subtract(UnixEpoch).TotalSeconds;

        }
        public static DateTimeOffset FromUnixTimeSeconds(long timestamp) {

            return UnixEpoch.AddSeconds(timestamp);

        }
        public static long ToUnixTimeMilliseconds(DateTimeOffset input) {

            return (long)input.ToUniversalTime().Subtract(UnixEpoch).TotalMilliseconds;

        }
        public static DateTimeOffset FromUnixTimeMilliseconds(long timestamp) {

            return UnixEpoch.AddMilliseconds(timestamp);

        }
        public static long CurrentUnixTimeSeconds() {

            return ToUnixTimeSeconds(DateTimeOffset.Now);

        }

        public static bool TryFormat(DateTimeOffset input, string format, out string result) {

#pragma warning disable CA1031 // Do not catch general exception types
            try {

                result = input.ToString(format);

                return true;

            }
            catch (Exception) {

                result = string.Empty;

                return false;

            }
#pragma warning restore CA1031 // Do not catch general exception types

        }

    }

}
using System;

namespace Gsemac.Utilities {

    public static class DateUtilities {

        // Public members

        public static DateTimeOffset MinimumUnixDate => new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

        public static long ToUnixTimeSeconds(DateTimeOffset input) {

            return (long)input.ToUniversalTime().Subtract(MinimumUnixDate).TotalSeconds;

        }
        public static DateTimeOffset FromUnixTimeSeconds(long timestamp) {

            return MinimumUnixDate.AddSeconds(timestamp);

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
using System;

namespace Gsemac.Utilities {

    public static class DateUtilities {

        // Public members

        public static DateTime MinimumUnixDateTime => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTimeSeconds(DateTime input) {

            return (long)input.ToUniversalTime().Subtract(MinimumUnixDateTime).TotalSeconds;

        }
        public static DateTime FromUnixTimeSeconds(long timestamp) {

            return MinimumUnixDateTime.AddSeconds(timestamp);

        }
        public static long CurrentUnixTimeSeconds() {

            return ToUnixTimeSeconds(DateTime.Now);

        }

    }

}
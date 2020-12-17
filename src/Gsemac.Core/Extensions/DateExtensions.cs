using System;

namespace Gsemac.Core.Extensions {

    public static class DateExtensions {

        public static long ToUnixTimeSeconds(this DateTime date) {

            return DateUtilities.ToUnixTimeSeconds(date);

        }
        public static long ToUnixTimeSeconds(this DateTimeOffset date) {

            return DateUtilities.ToUnixTimeSeconds(date);

        }
        public static long ToUnixTimeMilliseconds(this DateTime date) {

            return DateUtilities.ToUnixTimeMilliseconds(date);

        }
        public static long ToUnixTimeMilliseconds(this DateTimeOffset date) {

            return DateUtilities.ToUnixTimeMilliseconds(date);

        }

        public static string ToIso8601(this DateTime date) {

            return DateUtilities.ToIso8601(date);

        }
        public static string ToIso8601(this DateTimeOffset date) {

            return DateUtilities.ToIso8601(date);

        }

    }

}
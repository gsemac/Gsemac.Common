using System;

namespace Gsemac.Core.Extensions {

    public static class DateExtensions {

        public static long ToUnixTimeSeconds(this DateTime date) {

            return DateUtilities.ToUnixTimeSeconds(date);

        }
        public static long ToUnixTimeSeconds(this DateTimeOffset date) {

            return DateUtilities.ToUnixTimeSeconds(date);

        }

    }

}
using System;
using System.Text;

namespace Gsemac.Net.JavaScript.Extensions {

    public static class NumberExtensions {

        // Public members

        public static string ToString(this int number, int radix) {

            if (radix < 2 || radix > 36)
                throw new ArgumentOutOfRangeException(nameof(radix));

            switch (radix) {

                case 2:
                case 8:
                case 10:
                case 16:
                    return Convert.ToString(number, radix);

                case 36:
                    return ToStringBase36(number);

                default:
                    throw new ArgumentOutOfRangeException(nameof(radix));

            }

        }

        // Private members

        private static string ToStringBase36(int number) {

            if (number == 0)
                return "0";

            char[] dict = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();
            bool isNegative = number < 0;

            number = Math.Abs(number);

            StringBuilder resultBuilder = new StringBuilder();

            while (number > 0) {

                resultBuilder.Append(dict[number % 36]);

                number = (int)Math.Floor((double)number / 36);

            }

            if (isNegative)
                resultBuilder.Append("-");

            return new string(resultBuilder.ToString().ToCharArray().Reverse());

        }

    }

}
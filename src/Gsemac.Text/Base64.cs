using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Text {

    public static class Base64 {

        // Public members

        public static string EncodeString(string input) {

            return EncodeString(input, Encoding.UTF8);

        }
        public static string DecodeString(string input) {

            return DecodeString(input, Encoding.UTF8);

        }
        public static string EncodeString(string input, Encoding encoding) {

            return Convert.ToBase64String(encoding.GetBytes(input));

        }
        public static string DecodeString(string input, Encoding encoding) {

            // The default FromBase64String method will throw an exception if the input string is not padded.

            input = PadBase64String(input);

            return encoding.GetString(Convert.FromBase64String(input));

        }

        public static bool IsBase64String(string input) {

            // Slightly modified from the answer given here:
            // https://stackoverflow.com/a/54143400 (Tomas Kubes)

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = PadBase64String(input.Trim());

            return (input.Length % 4 == 0) &&
                Regex.IsMatch(input, @"^[a-z0-9\+/]*={0,3}$", RegexOptions.IgnoreCase);

        }

        // Private members

        private static string PadBase64String(string input) {

            if (string.IsNullOrWhiteSpace(input))
                return input;

            if (input.Length % 4 > 0)
                input = input.PadRight(input.Length + 4 - input.Length % 4, '=');

            return input;

        }

    }

}
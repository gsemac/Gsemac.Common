using System;

namespace Gsemac.Net.JavaScript.Extensions {

    public static class JsStringExtensions {

        public static string CharAt(this string str, int index) {

            if (index >= 0 && index < str.Length)
                return str[index].ToString();

            return "";

        }
        public static int CharCodeAt(this string str, int index) {

            if (index >= 0 && index < str.Length)
                return char.ConvertToUtf32(str, index);

            return -1;


        }
        public static int IndexOf(this string str, int findValue) {

            return str.IndexOf(findValue);

        }
        public static int IndexOf(this string str, int findValue, int fromIndex) {

            return str.IndexOf(findValue, fromIndex);

        }
        public static string Slice(this string str, int index) {

            if (index >= 0)
                return (index < str.Length) ? str.Substring(index, str.Length - index) : string.Empty;
            else
                return (-index < str.Length) ? str.Substring(str.Length + index, -index) : str;

        }
        public static string Slice(this string str, int startIndex, int endIndex) {

            if (startIndex < 0)
                startIndex = Math.Max(str.Length + startIndex, 0);

            if (endIndex < 0)
                endIndex = str.Length + endIndex;

            return str.Substring(startIndex, endIndex - startIndex);

        }
        public static string Substr(this string str, int start, int length) {

            return Substring(str, Math.Min(length, str.Length - start));

        }
        public static string Substring(this string str, int start) {

            if (start < 0)
                start = 0;

            return str.Substring(start);

        }
        public static string Substring(this string str, int start, int end) {

            if (end < 0)
                return Substring(str, start);

            if (start > end) {

                int temp = end;

                end = start;
                start = temp;

            }

            return str.Substring(start, end - start);

        }

    }

}
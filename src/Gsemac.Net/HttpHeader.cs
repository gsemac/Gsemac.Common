using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net {

    public class HttpHeader :
        IHttpHeader {

        public string Name { get; }
        public string Value { get; }

        public HttpHeader(string name, string value) {

            Name = name;
            Value = value;

        }

        public static HttpHeader Parse(string httpHeader) {

            if (TryParse(httpHeader, out HttpHeader result))
                return result;
            else
                throw new ArgumentException("The given string was not a valid HTTP header.", nameof(httpHeader));


        }
        public static bool TryParse(string httpHeader, out HttpHeader result) {

            Match httpHeaderMatch = Regex.Match(httpHeader, @"^(.+?):\s(.+?)$");

            if (httpHeaderMatch.Success) {

                string name = httpHeaderMatch.Groups[1].Value;
                string value = httpHeaderMatch.Groups[2].Value;

                result = new HttpHeader(name, value);

            }
            else {

                result = null;

            }

            return result != null;

        }

        public override string ToString() {

            return $"{Name}: {Value}";

        }

    }

}
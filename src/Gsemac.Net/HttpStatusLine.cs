using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net {

    public class HttpStatusLine :
        IHttpStatusLine {

        public Version ProtocolVersion { get; }
        public HttpStatusCode StatusCode { get; }
        public string StatusDescription { get; }

        public HttpStatusLine(Version protocolVersion, HttpStatusCode statusCode, string statusDescription) {

            ProtocolVersion = protocolVersion;
            StatusCode = statusCode;
            StatusDescription = statusDescription;

        }

        public static IHttpStatusLine Parse(string statusLine) {

            if (TryParse(statusLine, out IHttpStatusLine httpResponseStatus))
                return httpResponseStatus;
            else
                throw new ArgumentException("The given string was not a valid HTTP status line.", nameof(statusLine));

        }
        public static bool TryParse(string statusLine, out IHttpStatusLine result) {

            Match statusLineMatch = Regex.Match(statusLine, @"^HTTP\/(\d+(?:\.\d+)?)\s*(\d+)\s*(.+?)$");

            if (statusLineMatch.Success) {

                string protocolVersionStr = statusLineMatch.Groups[1].Value;
                string[] versionNumsStr = protocolVersionStr.Split('.');

                int major = int.Parse(versionNumsStr[0]);
                int minor = versionNumsStr.Length > 1 ? int.Parse(versionNumsStr[1]) : 0;

                Version protocolVersion = new Version(major, minor);
                HttpStatusCode statusCode = HttpStatusCode.OK;

                if (int.TryParse(statusLineMatch.Groups[2].Value, out int statusCodeInt))
                    statusCode = (HttpStatusCode)statusCodeInt;

                string statusDescription = statusLineMatch.Groups[3].Value;

                result = new HttpStatusLine(protocolVersion, statusCode, statusDescription);

            }
            else {

                result = null;

            }

            return result is object;

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append("HTTP/");
            sb.Append(ProtocolVersion.Major);

            if (ProtocolVersion.Minor > 0) {

                sb.Append(".");
                sb.Append(ProtocolVersion.Minor);

            }

            sb.Append(" ");

            sb.Append((int)StatusCode);

            if (!string.IsNullOrWhiteSpace(StatusDescription)) {

                sb.Append(" ");

                sb.Append(StatusDescription);

            }

            return sb.ToString();

        }

    }

}
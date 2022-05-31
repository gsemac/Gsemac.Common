using Gsemac.Net.Properties;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http {

    public class HttpStatusLine :
        IHttpStatusLine {

        // Public members

        public Version ProtocolVersion { get; }
        public HttpStatusCode StatusCode { get; }
        public string StatusDescription { get; }

        public HttpStatusLine(HttpStatusCode statusCode) :
           this(DefaultProtocolVersion, statusCode, HttpUtilities.GetStatusDescription(statusCode)) {
        }
        public HttpStatusLine(HttpStatusCode statusCode, string statusDescription) :
            this(DefaultProtocolVersion, statusCode, statusDescription) {
        }
        public HttpStatusLine(Version protocolVersion, HttpStatusCode statusCode, string statusDescription) {

            if (protocolVersion is null)
                throw new ArgumentNullException(nameof(protocolVersion));

            ProtocolVersion = protocolVersion;
            StatusCode = statusCode;
            StatusDescription = statusDescription;

        }

        public static HttpStatusLine Parse(string statusLine) {

            if (TryParse(statusLine, out HttpStatusLine httpResponseStatus))
                return httpResponseStatus;
            else
                throw new ArgumentException(ExceptionMessages.InvalidHttpStatusLine, nameof(statusLine));

        }
        public static bool TryParse(string statusLine, out HttpStatusLine result) {

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

        // Private members

        private static Version DefaultProtocolVersion => new Version(1, 1);

    }

}
using Gsemac.Net.Properties;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http {

    public class HttpRequestLine :
        IHttpRequestLine {

        // Public members

        public string Method { get; }
        public Uri RequestUri { get; }
        public Version ProtocolVersion { get; }

        public HttpRequestLine(string method, Uri requestUri, Version protocolVersion) {

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            if (protocolVersion is null)
                throw new ArgumentNullException(nameof(protocolVersion));

            Method = method;
            RequestUri = requestUri;
            ProtocolVersion = protocolVersion;

        }

        public static HttpRequestLine Parse(string requestLine) {

            if (TryParse(requestLine, out HttpRequestLine httpResponseStatus))
                return httpResponseStatus;
            else
                throw new ArgumentException(ExceptionMessages.InvalidHttpStatusLine, nameof(requestLine));

        }
        public static bool TryParse(string requestLine, out HttpRequestLine result) {

            Match requestLineMatch = Regex.Match(requestLine, @"^(?<method>.+?)\s(?<uri>.+?)\sHTTP\/(?<version>\d+\.?\d+)$");

            if (requestLineMatch.Success && Uri.TryCreate(requestLineMatch.Groups["uri"].Value, UriKind.RelativeOrAbsolute, out Uri uri)) {

                string protocolVersionStr = requestLineMatch.Groups["version"].Value;
                string[] versionNumsStr = protocolVersionStr.Split('.');

                int major = int.Parse(versionNumsStr[0]);
                int minor = versionNumsStr.Length > 1 ? int.Parse(versionNumsStr[1]) : 0;

                Version protocolVersion = new Version(major, minor);
                string method = requestLineMatch.Groups["method"].Value;

                result = new HttpRequestLine(method, uri, protocolVersion);

            }
            else {

                result = null;

            }

            return result is object;

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Method);
            sb.Append(' ');
            sb.Append(RequestUri.OriginalString);
            sb.Append(' ');
            sb.Append("HTTP/");

            sb.Append(ProtocolVersion.Major);

            if (ProtocolVersion.Minor > 0) {

                sb.Append('.');
                sb.Append(ProtocolVersion.Minor);

            }

            return sb.ToString();

        }

    }

}
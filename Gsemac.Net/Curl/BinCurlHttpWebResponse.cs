using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Curl {

    public class BinCurlHttpWebResponse :
        HttpWebResponseBase {

        // Public members

        public override Uri ResponseUri => responseUri;
        public override CookieCollection Cookies {
            get => cookies.GetCookies(ResponseUri);
            set => cookies.Add(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlHttpWebResponse"/> class.
        /// </summary>
        /// <param name="responseUri">URI of the response page.</param>
        /// <param name="responseStream">Stream containing response content.</param>
        public BinCurlHttpWebResponse(Uri responseUri, BinCurlProcessStream responseStream) :
            this(responseUri, responseStream, null, string.Empty) {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlHttpWebResponse"/> class.
        /// </summary>
        /// <param name="responseUri">URI of the response page.</param>
        /// <param name="responseStream">Stream containing response content.</param>
        /// <param name="protocolVersion">HTTP protocol version used to fetch the response.</param>
        /// <param name="method">Method used to fetch the response.</param>
        public BinCurlHttpWebResponse(Uri responseUri, BinCurlProcessStream responseStream, Version protocolVersion, string method) :
            base(responseUri, responseStream) {

            // Set member variables.

            ProtocolVersion = protocolVersion;
            Method = method;

            // Read headers from the stream before letting the user access it.

            ReadHeadersFromStream(GetResponseStream());

            CharacterSet = GetCharSet();

            // If we got an error status code (or nothing at all), throw an exception.

            if (StatusCode == 0) {

                // We didn't read a status code from the stream at all.
                throw new WebException("Did not receive a valid HTTP response.", null, WebExceptionStatus.ServerProtocolViolation, this);

            }
            else if (!((int)StatusCode).ToString().StartsWith("2")) {

                // We got a response, but didn't get a success code.
                throw new WebException(string.Format("Server responded with error.", (int)StatusCode), null, WebExceptionStatus.ProtocolError, this);

            }

        }

        // Private members

        private Uri responseUri;
        private readonly CookieContainer cookies = new CookieContainer();

        private string ReadLineFromStream(Stream stream) {

            using (MemoryStream ms = new MemoryStream()) {

                while (true) {

                    int nextByte = stream.ReadByte();

                    if (nextByte == -1 || nextByte == '\n')
                        break;
                    else
                        ms.WriteByte((byte)nextByte);

                }

                string line = Encoding.UTF8.GetString(ms.ToArray());

                return line;

            }

        }
        private void ReadHeadersFromStream(Stream stream) {

            while (true) {

                string line = ReadLineFromStream(stream);

                if (string.IsNullOrWhiteSpace(line)) {

                    // Curl will output multiple sets of headers if it gets redirected.
                    // If we got a "location" header, reset headers and continue reading.

                    string locationValue = Headers[HttpResponseHeader.Location];

                    if (!string.IsNullOrEmpty(locationValue)) {

                        Headers.Clear();

                        if (!Uri.TryCreate(locationValue, UriKind.Absolute, out Uri locationUri))
                            Uri.TryCreate(ResponseUri.GetLeftPart(UriPartial.Authority) + locationValue, UriKind.Absolute, out locationUri);

                        if (locationUri != null)
                            responseUri = locationUri;

                    }
                    else
                        break;

                }
                else
                    ParseAndAddHeader(line);

            }

        }
        private void ParseAndAddHeader(string headerText) {

            Match headerMatch = Regex.Match(headerText, @"^(.+?):\s*(.+?)$");

            if (headerMatch.Success) {

                string header = headerMatch.Groups[1].Value;
                string value = headerMatch.Groups[2].Value;

                if (header.Equals("set-cookie", StringComparison.OrdinalIgnoreCase))
                    cookies.SetCookies(ResponseUri, value);
                else
                    Headers.Add(header, value);

            }
            else {

                // Try reading HTTP status line instead.

                if (HttpStatusLine.TryParse(headerText, out HttpStatusLine statusLine)) {

                    ProtocolVersion = statusLine.ProtocolVersion;
                    StatusCode = statusLine.StatusCode;
                    StatusDescription = statusLine.StatusDescription;

                }

            }

        }
        private string GetCharSet() {

            string charSet = string.Empty;
            string contentType = Headers[HttpResponseHeader.ContentType];

            if (!string.IsNullOrEmpty(contentType)) {

                Match m = Regex.Match(contentType, @"charset=([^\s]+)");

                if (m.Success)
                    charSet = m.Groups[1].Value;

            }

            return charSet;

        }

    }

}
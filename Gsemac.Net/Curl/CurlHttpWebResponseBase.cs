using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Curl {

    public abstract class CurlHttpWebResponseBase :
        HttpWebResponseBase {

        // Public members

        public override CookieCollection Cookies {
            get => cookies.GetCookies(ResponseUri);
            set => cookies.Add(value);
        }
        public override Uri ResponseUri => responseUri;

        // Protected members

        protected CurlHttpWebResponseBase(IHttpWebRequest parentRequest, Stream responseStream) :
            base(parentRequest.RequestUri, responseStream) {

            this.responseUri = parentRequest.RequestUri;
            this.maximumAutomaticRedirections = parentRequest.AllowAutoRedirect ? parentRequest.MaximumAutomaticRedirections : 0;

            Method = parentRequest.Method;
            ProtocolVersion = parentRequest.ProtocolVersion;

            // Read headers from the stream before returning to the caller so our property values are valid.

            ReadHttpHeaders(responseStream);

            // Check status code for success.

            if (StatusCode == 0) {

                // We didn't read a status code from the stream at all.

                throw new WebException("Received an empty response.", null, WebExceptionStatus.ServerProtocolViolation, this);

            }
            else if (!((int)StatusCode).ToString(CultureInfo.InvariantCulture).StartsWith("2")) {

                // We got a response, but didn't get a success code.

                throw new WebException($"The remote server returned an error: ({(int)StatusCode}) {StatusDescription}.", null, WebExceptionStatus.ProtocolError, this);

            }

        }

        // Private members

        private Uri responseUri;
        private readonly CookieContainer cookies = new CookieContainer();
        private readonly int maximumAutomaticRedirections;

        private void ReadHttpHeaders(Stream responseStream) {

            using (IHttpResponseReader reader = new HttpResponseReader(responseStream)) {

                try {

                    // Curl will output multiple sets of headers if we are redirected.

                    bool readHeaders = true;
                    int numberOfRedirects = 0;

                    while (readHeaders) {

                        Headers.Clear();

                        IHttpStatusLine statusLine = reader.ReadStatusLine();

                        ProtocolVersion = statusLine.ProtocolVersion;
                        StatusCode = statusLine.StatusCode;
                        StatusDescription = statusLine.StatusDescription;

                        foreach (IHttpHeader header in reader.ReadHeaders())
                            Headers.Add(header.Name, header.Value);

                        // Update response URI and continue reading headers if we were redirected.

                        string location = Headers[HttpResponseHeader.Location];
                        bool hasLocation = !string.IsNullOrWhiteSpace(location);

                        readHeaders = hasLocation;

                        if (hasLocation) {

                            if (!Uri.TryCreate(location, UriKind.Absolute, out Uri locationUri))
                                Uri.TryCreate(ResponseUri.GetLeftPart(UriPartial.Authority) + location, UriKind.Absolute, out locationUri);

                            if (!(locationUri is null))
                                responseUri = locationUri;

                            readHeaders = numberOfRedirects++ < maximumAutomaticRedirections;

                        }

                    }

                    // Get charset.

                    string contentType = Headers[HttpResponseHeader.ContentType];

                    if (!string.IsNullOrWhiteSpace(contentType)) {

                        Match charsetMatch = Regex.Match(contentType, @"charset=([^\s]+)");

                        if (charsetMatch.Success)
                            CharacterSet = charsetMatch.Groups[1].Value;

                    }

                    // Get cookies.

                    string setCookie = Headers[HttpResponseHeader.SetCookie];

                    if (!string.IsNullOrWhiteSpace(setCookie))
                        cookies.SetCookies(ResponseUri, setCookie);

                }
                catch (ArgumentException) {

                    throw new ProtocolViolationException("The response was malformed.");

                }

            }

        }

    }

}
﻿using Gsemac.Net.Extensions;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Curl {

    internal abstract class CurlHttpWebResponseBase :
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

            responseUri = parentRequest.RequestUri;

            Method = parentRequest.Method;
            ProtocolVersion = parentRequest.ProtocolVersion;

            // Read headers from the stream before returning to the caller so our property values are valid.

            ReadHttpHeaders(responseStream);

            // Check status code for success.

            if (StatusCode == 0) {

                // We didn't read a status code from the stream at all.

                throw new WebException("Received an empty response.", null, WebExceptionStatus.ServerProtocolViolation, this);

            }
            else if ((int)StatusCode >= 400 && (int)StatusCode < 600) {

                // We got a response, but didn't get a success code.

                throw new WebException($"The remote server returned an error: ({(int)StatusCode}) {StatusDescription}.", null, WebExceptionStatus.ProtocolError, this);

            }

        }

        // Private members

        private Uri responseUri;
        private readonly CookieContainer cookies = new CookieContainer();

        private void ReadHttpHeaders(Stream responseStream) {

            using (IHttpResponseReader reader = new HttpResponseReader(responseStream)) {

                // Curl will output multiple sets of headers if we are redirected.

                bool readHeaders = true;

                while (readHeaders) {

                    // We might fail to read the status line if we had a redirect that wasn't followed (i.e. "AllowAutoRedirect" is false).
                    // In that case, there are no other responses to read.

                    if (!reader.TryReadStatusLine(out IHttpStatusLine statusLine))
                        break;

                    ProtocolVersion = statusLine.ProtocolVersion;
                    StatusCode = statusLine.StatusCode;
                    StatusDescription = statusLine.StatusDescription;

                    Headers.Clear();

                    foreach (IHttpHeader header in reader.ReadHeaders())
                        Headers.Add(header.Name, header.Value);

                    // Update response URI and continue reading headers if we were redirected.

                    bool wasRedirected = Headers.TryGetHeaderValue(HttpResponseHeader.Location, out string locationHeader);
                    Uri locationUri = null;

                    readHeaders = wasRedirected;

                    if (wasRedirected) {

                        if (!Uri.TryCreate(responseUri, locationHeader, out locationUri))
                            throw new WebException("Cannot handle redirect from HTTP/HTTPS protocols to other dissimilar ones.", null, WebExceptionStatus.ProtocolError, this);

                    }

                    // Get cookies.

                    if (Headers.TryGetHeaderValue(HttpResponseHeader.SetCookie, out string setCookieHeader))
                        cookies.SetCookies(responseUri, setCookieHeader);

                    // Update the response URI.
                    // This is done after reading the "set-cookie" header so that the cookies are applied to the original domain instead of the new one.

                    if (locationUri is object)
                        responseUri = locationUri;

                }

                // Get charset.

                if (Headers.TryGetHeaderValue(HttpResponseHeader.ContentType, out string contentTypeHeader)) {

                    Match charsetMatch = Regex.Match(contentTypeHeader, @"charset=([^\s]+)");

                    if (charsetMatch.Success)
                        CharacterSet = charsetMatch.Groups[1].Value;

                }

            }

        }

    }

}
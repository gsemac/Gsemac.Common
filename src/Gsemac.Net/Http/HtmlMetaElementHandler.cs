using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Http {

    public class HtmlMetaElementHandler :
        HttpWebRequestHandler {

        // Protected members

        protected override IHttpWebResponse GetResponse(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            IHttpWebResponse response = base.GetResponse(request, cancellationToken);

            // If the response is HTML content, read the contents of the "head" element.
            // We will analyze the "meta" elements inside and update the response object accordingly.

            if (response is object && !string.IsNullOrEmpty(response.ContentType) && response.ContentType.StartsWith("text/html", StringComparison.OrdinalIgnoreCase)) {

                // We could just partially read the stream and wrap the response stream in one that allows it to be reset, but this is dangerous, because the user might
                // not call GetResponseStream and never end up closing the stream. Instead, we'll read the entire stream into memory (MemoryStream does not need to be disposed).

                MemoryStream memoryStream = new MemoryStream();

                using (Stream responseStream = response.GetResponseStream()) {

                    if (responseStream is object)
                        responseStream.CopyTo(memoryStream);

                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                HtmlDocument htmlDocument = new HtmlDocument {
                    OptionStopperNodeName = "head",
                };

                htmlDocument.Load(memoryStream);

                // We can extract the character set directly from HtmlDocument (if one was specified).
                // If no character set was detected, we'll use the one already set for the response as specified in the "content-type" header. 

                string declaredEncoding = htmlDocument.DeclaredEncoding?.WebName ?? string.Empty;

                // Read the "meta" elements with an "http-equiv" attribute and set the appropriate headers.
                // There are only a few header names which are considered valid.
                // https://developer.mozilla.org/en-US/docs/Web/HTML/Element/meta

                foreach (HtmlNode httpEquivNode in GetMetaHttpEquivNodes(htmlDocument)) {

                    string httpEquiv = httpEquivNode.GetAttributeValue("http-equiv", string.Empty).ToLowerInvariant();
                    string content = httpEquivNode.GetAttributeValue("content", string.Empty);

                    switch (httpEquiv) {

                        case "content-type":
                            response.Headers["content-type"] = content;
                            break;

                        case "refresh":
                            response.Headers["refresh"] = content;
                            break;

                    }

                }

                // Return a new response object with the new stream.

                memoryStream.Seek(0, SeekOrigin.Begin);

                response = new HtmlMetaElementHandlerHttpWebResponseWrapper(response, memoryStream, declaredEncoding);

            }

            return response;

        }

        // Private members

        private class HtmlMetaElementHandlerHttpWebResponseWrapper :
            WebResponse,
            IHttpWebResponse {

            // Public members

            public string ContentEncoding => baseResponse.ContentEncoding;
            public Version ProtocolVersion => baseResponse.ProtocolVersion;
            public string StatusDescription => baseResponse.StatusDescription;
            public HttpStatusCode StatusCode => baseResponse.StatusCode;
            public DateTime LastModified => baseResponse.LastModified;
            public string Server => baseResponse.Server;
            public string CharacterSet => string.IsNullOrWhiteSpace(characterSet) ? baseResponse.CharacterSet : characterSet;
            public string Method => baseResponse.Method;
            public CookieCollection Cookies {
                get => baseResponse.Cookies;
                set => baseResponse.Cookies = value;
            }

            public override bool IsFromCache => baseResponse.IsFromCache;
            public override bool IsMutuallyAuthenticated => baseResponse.IsMutuallyAuthenticated;
            public override long ContentLength {
                get => baseResponse.ContentLength;
                set => baseResponse.ContentLength = value;
            }
            public override string ContentType {
                get => baseResponse.ContentType;
                set => baseResponse.ContentType = value;
            }
            public override Uri ResponseUri => baseResponse.ResponseUri;
            public override WebHeaderCollection Headers => baseResponse.Headers;

            public HtmlMetaElementHandlerHttpWebResponseWrapper(IHttpWebResponse baseResponse, Stream responseStream, string characterSet) {

                if (baseResponse is null)
                    throw new ArgumentNullException(nameof(baseResponse));

                if (responseStream is null)
                    throw new ArgumentNullException(nameof(responseStream));

                this.baseResponse = baseResponse;
                this.responseStream = responseStream;
                this.characterSet = characterSet;

            }

            public string GetResponseHeader(string headerName) {

                return baseResponse.GetResponseHeader(headerName);

            }

            public override void Close() {

                baseResponse.Close();

            }
            public override Stream GetResponseStream() {

                return responseStream;

            }

            // Private members

            private readonly IHttpWebResponse baseResponse;
            private readonly Stream responseStream;
            private readonly string characterSet;

        }

        private static IEnumerable<HtmlNode> GetMetaHttpEquivNodes(HtmlDocument htmlDocument) {

            if (htmlDocument is null)
                throw new ArgumentNullException(nameof(htmlDocument));

            return htmlDocument.DocumentNode?.SelectNodes("//meta[@http-equiv]") ?? Enumerable.Empty<HtmlNode>();

        }

    }

}
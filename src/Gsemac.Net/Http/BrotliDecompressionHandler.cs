using BrotliSharpLib;
using Gsemac.Net.Http.Headers;
using Gsemac.Polyfills.System.Reflection;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Http {

    public sealed class BrotliDecompressionHandler :
        HttpWebRequestHandler {

        // Protected members

        protected override IHttpWebResponse GetResponse(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            IHttpWebResponse response = base.GetResponse(request, cancellationToken);

            if (IsBrotliCompressed(response))
                return new BrotliDecompressionHttpWebResponseDecorator(response);

            return response;

        }

        // Private members

        private sealed class BrotliDecompressionHttpWebResponseDecorator :
            HttpWebResponseDecoratorBase {

            // Public members

            public BrotliDecompressionHttpWebResponseDecorator(IHttpWebResponse response) :
                 base(response) {

                this.response = response;

            }

            public override Stream GetResponseStream() {

                return new BrotliStream(response.GetResponseStream(), CompressionMode.Decompress, leaveOpen: false);

            }

            // Private members

            private readonly IHttpWebResponse response;

        }

        private static bool IsBrotliCompressed(IHttpWebResponse response) {

            if (response is null)
                throw new ArgumentNullException(nameof(response));

            // While the "content-encoding" header can give us some clue about the format of the response stream, it's not guaranteed to be accurate.
            // Unfortunately, there are no magic bytes we can use to detect a brotli stream.

            // It is possible for WebResponse implementations to opt-out of decompression altogether with the "ResponseStreamAlreadyDecompressed" attribute.
            // This is useful for implementations that handle their own decompression.

            if (response is WebResponse webResponse) {

                webResponse = HttpWebRequestUtilities.GetInnermostWebResponse(webResponse);

                ResponseStreamAlreadyDecompressedAttribute alreadyDecompressedAttribute = webResponse.GetType()
                    .GetCustomAttribute<ResponseStreamAlreadyDecompressedAttribute>();

                if (alreadyDecompressedAttribute is object)
                    return false;

            }

            return AcceptEncodingHeaderValue.TryParse(response.ContentEncoding, out AcceptEncodingHeaderValue result) &&
                result.DecompressionMethods.HasFlag((DecompressionMethods)Polyfills.System.Net.DecompressionMethods.Brotli);

        }

    }

}
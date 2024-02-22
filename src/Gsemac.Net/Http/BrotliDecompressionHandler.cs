using BrotliSharpLib;
using Gsemac.Net.Http.Headers;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Http {

    public sealed class BrotliDecompressionHandler :
        HttpWebRequestHandler {

        // Protected members

        protected override IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            IHttpWebResponse response = base.Send(request, cancellationToken);

            if (IsBrotliEncoded(response))
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

        private static bool IsBrotliEncoded(IHttpWebResponse response) {

            if (response is null)
                throw new ArgumentNullException(nameof(response));

            return (AcceptEncodingHeaderValue.TryParse(response.ContentEncoding, out AcceptEncodingHeaderValue result) &&
                    result.EncodingMethods.HasFlag((DecompressionMethods)Polyfills.System.Net.DecompressionMethods.Brotli));

        }

    }

}
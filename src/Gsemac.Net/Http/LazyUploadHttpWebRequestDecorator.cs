using Gsemac.IO.Extensions;
using System;
using System.IO;
using System.Net;

namespace Gsemac.Net.Http {

    // The default HttpWebRequest implementation will open a ConnectStream and start streaming data immediately when GetRequestStream is called.
    // The downside of this is that there's no way to access the POST data in order to retry the request if something goes wrong partway through.
    // This decorator will upload the stream lazily so it can still be read later, at the cost of buffering the entire stream in memory.

    public class LazyUploadHttpWebRequestDecorator :
        HttpWebRequestDecoratorBase {

        // Public members

        public override long ContentLength {
            get => GetContentLength();
            set => SetContentLength(value);
        }

        public LazyUploadHttpWebRequestDecorator(IHttpWebRequest innerHttpWebRequest) :
            this(innerHttpWebRequest, () => new MemoryStream()) {
        }
        public LazyUploadHttpWebRequestDecorator(IHttpWebRequest innerHttpWebRequest, Func<Stream> streamFactory) :
          base(innerHttpWebRequest) {

            if (streamFactory is null)
                throw new ArgumentNullException(nameof(streamFactory));

            requestStream = new Lazy<Stream>(() => streamFactory());

        }

        public override Stream GetRequestStream() {

            return requestStream.Value;

        }
        public override WebResponse GetResponse() {

            if (requestStream.IsValueCreated) {

                // We need to copy the request stream before copying it to the base request stream because the user is likely to have disposed it.
                // Fortunately, the ToArray method is still usable after the MemoryStream has been disposed.

                using (Stream copiedRequestStream = new MemoryStream(requestStream.Value.ToArray()))
                using (Stream baseRequestStream = base.GetRequestStream())
                    copiedRequestStream.CopyTo(baseRequestStream);

            }

            return base.GetResponse();

        }

        // Private members

        private readonly Lazy<Stream> requestStream;
        private bool isContentLengthManuallySet = false;

        private long GetContentLength() {

            if (isContentLengthManuallySet)
                return base.ContentLength;

            if (!requestStream.IsValueCreated)
                return -1;

            if (requestStream.Value.CanSeek)
                return requestStream.Value.Length;

            // Reading the entire stream into an array just to get the length is very inefficient.
            // However, it seems to be the only way we can access the length of a disposed MemoryStream instance.

            return requestStream.Value.ToArray().Length;

        }
        private void SetContentLength(long value) {

            base.ContentLength = value;

            isContentLengthManuallySet = true;

        }

    }

}
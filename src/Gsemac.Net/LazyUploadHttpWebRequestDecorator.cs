using System;
using System.IO;
using System.Net;

namespace Gsemac.Net {

    // The default HttpWebRequest implementation will open a ConnectStream and start streaming data immediately when GetRequestStream is called.
    // The downside of this is that there's no way to access the POST data in order to retry the request if something goes wrong partway through.
    // This decorator will upload the stream lazily so it can still be read later, at the cost of buffering the entire stream in memory.

    public class LazyUploadHttpWebRequestDecorator :
        HttpWebRequestDecoratorBase {

        // Public members

        public LazyUploadHttpWebRequestDecorator(IHttpWebRequest innerHttpWebRequest) :
            base(innerHttpWebRequest) {
        }

        public override Stream GetRequestStream() {

            return requestStream.Value;

        }
        public override WebResponse GetResponse() {

            if (requestStream.IsValueCreated) {

                // We need to copy the request stream before copying it to the base request stream because the user is likely to have disposed it.
                // Fortunately, the ToArray method is still usuable after the MemoryStream has been disposed.

                using (Stream copiedRequestStream = new MemoryStream(requestStream.Value.ToArray()))
                using (Stream baseRequestStream = base.GetRequestStream())
                    copiedRequestStream.CopyTo(baseRequestStream);

            }

            return base.GetResponse();

        }

        // Private members

        private readonly Lazy<MemoryStream> requestStream = new Lazy<MemoryStream>(() => new MemoryStream());

    }

}
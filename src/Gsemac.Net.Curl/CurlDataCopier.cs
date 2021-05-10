using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Gsemac.Net.Curl {

    public class CurlDataCopier :
        ICurlDataCopier {

        // Public members

        public WriteFunctionCallback Header { get; }
        public ReadFunctionCallback Read { get; }
        public WriteFunctionCallback Write { get; }
        public ProgressFunctionCallback Progress { get; }

        public CurlDataCopier() {

            Header = new WriteFunctionCallback(HeaderFunctionImpl);
            Read = new ReadFunctionCallback(ReadFunctionImpl);
            Write = new WriteFunctionCallback(WriteFunctionImpl);
            Progress = new ProgressFunctionCallback(ProgressFunctionImpl);

        }
        public CurlDataCopier(Stream responseStream) :
            this(responseStream, CancellationToken.None) {
        }
        public CurlDataCopier(Stream responseStream, CancellationToken cancellationToken) :
            this() {

            if (responseStream is null)
                throw new ArgumentNullException(nameof(responseStream));

            this.responseStream = responseStream;
            this.cancellationToken = cancellationToken;

        }
        public CurlDataCopier(Stream requestStream, Stream responseStream, CancellationToken cancellationToken) :
            this(responseStream, cancellationToken) {

            if (requestStream is null)
                throw new ArgumentNullException(nameof(requestStream));

            this.requestStream = requestStream;

        }

        // Private members

        private readonly Stream responseStream;
        private readonly Stream requestStream;
        private readonly CancellationToken cancellationToken = CancellationToken.None;

        private UIntPtr HeaderFunctionImpl(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            return WriteFunctionImpl(data, size, nmemb, userdata);

        }
        private UIntPtr ReadFunctionImpl(IntPtr buffer, UIntPtr size, UIntPtr nitems, IntPtr userdata) {

            if (requestStream is null)
                return UIntPtr.Zero;

            int length = (int)size * (int)nitems;
            byte[] readBuffer = new byte[length];

            int bytesRead = requestStream.Read(readBuffer, 0, length);

            Marshal.Copy(readBuffer, 0, buffer, length);

            return (UIntPtr)bytesRead;

        }
        private UIntPtr WriteFunctionImpl(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            int length = (int)size * (int)nmemb;
            byte[] buffer = new byte[length];

            Marshal.Copy(data, buffer, 0, length);

            if (responseStream != null)
                responseStream.Write(buffer, 0, length);

            return (UIntPtr)length;

        }
        private int ProgressFunctionImpl(IntPtr clientp, double dltotal, double dlnow, double ultotal, double ulnow) {

            // Returning a non-zero value will cause the transfer to abort with code CURLE_ABORTED_BY_CALLBACK.

            if (cancellationToken.IsCancellationRequested)
                return -1;

            return 0;

        }

    }

}
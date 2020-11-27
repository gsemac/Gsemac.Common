using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Gsemac.Net.Curl {

    public class CurlDataCopier {

        // Public members

        public CurlDataCopier(Stream outputStream) :
            this(outputStream, CancellationToken.None) {
        }
        public CurlDataCopier(Stream outputStream, CancellationToken cancellationToken) {

            this.outputStream = outputStream;
            this.cancellationToken = cancellationToken;

            // Delegates must be kept alive (i.e. not garbage collected) unil there is no chance of them being called again.
            // To make sure they are kept alive for the duration of the request, they are stored in member variables.

            this.headerFunction = new WriteFunctionDelegate(HeaderFunction);
            this.writeFunction = new WriteFunctionDelegate(WriteFunction);
            this.progressFunction = new ProgressFunctionDelegate(ProgressFunction);

        }
        public CurlDataCopier(CurlEasyHandle easyHandle, Stream outputStream, CancellationToken cancellationToken) :
            this(outputStream, cancellationToken) {

            LibCurl.EasySetOpt(easyHandle, CurlOption.HeaderFunction, headerFunction);
            LibCurl.EasySetOpt(easyHandle, CurlOption.WriteFunction, writeFunction);
            LibCurl.EasySetOpt(easyHandle, CurlOption.ProgessFunction, progressFunction);

        }

        public UIntPtr HeaderFunction(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            return WriteFunction(data, size, nmemb, userdata);

        }
        public UIntPtr WriteFunction(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            int length = (int)size * (int)nmemb;
            byte[] buffer = new byte[length];

            Marshal.Copy(data, buffer, 0, length);

            outputStream.Write(buffer, 0, length);

            return (UIntPtr)length;

        }
        public int ProgressFunction(IntPtr clientp, double dltotal, double dlnow, double ultotal, double ulnow) {

            // Returning a non-zero value will cause the transfer to abort with code CURLE_ABORTED_BY_CALLBACK.

            if (cancellationToken.IsCancellationRequested)
                return -1;

            return 0;

        }

        // Private members

        private readonly Stream outputStream;
        private readonly CancellationToken cancellationToken;
        private readonly WriteFunctionDelegate headerFunction;
        private readonly WriteFunctionDelegate writeFunction;
        private readonly ProgressFunctionDelegate progressFunction;

    }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        }
        public CurlDataCopier(CurlEasyHandle easyHandle, Stream outputStream, CancellationToken cancellationToken) :
            this(outputStream, cancellationToken) {

            LibCurl.EasySetOpt(easyHandle, CurlOption.HeaderFunction, WriteCallback);
            LibCurl.EasySetOpt(easyHandle, CurlOption.WriteFunction, WriteCallback);
            LibCurl.EasySetOpt(easyHandle, CurlOption.ProgessFunction, ProgressCallback);

        }

        public UIntPtr WriteCallback(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            int length = (int)size * (int)nmemb;
            byte[] buffer = new byte[length];

            Marshal.Copy(data, buffer, 0, length);

            outputStream.Write(buffer, 0, length);

            return (UIntPtr)length;

        }
        public int ProgressCallback(IntPtr clientp, double dltotal, double dlnow, double ultotal, double ulnow) {

            // Returning a non-zero value will cause the transfer to abort with code CURLE_ABORTED_BY_CALLBACK.

            if (cancellationToken.IsCancellationRequested)
                return -1;

            return 0;

        }

        // Private members

        private readonly Stream outputStream;
        private readonly CancellationToken cancellationToken;

    }

}
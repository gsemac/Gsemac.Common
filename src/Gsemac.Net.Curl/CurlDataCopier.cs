using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Gsemac.Net.Curl {

    public class CurlDataCopier {

        // Public members

        public CurlDataCopier(Stream writeStream) :
            this(writeStream, CancellationToken.None) {
        }
        public CurlDataCopier(Stream writeStream, CancellationToken cancellationToken) {

            this.writeStream = writeStream;
            this.cancellationToken = cancellationToken;

            // Delegates must be kept alive (i.e. not garbage collected) unil there is no chance of them being called again.
            // To make sure they are kept alive for the duration of the request, they are stored in member variables.

            this.headerFunction = new WriteFunctionDelegate(HeaderFunctionImpl);
            this.readFunction = new ReadFunctionDelegate(ReadFunctionImpl);
            this.writeFunction = new WriteFunctionDelegate(WriteFunctionImpl);
            this.progressFunction = new ProgressFunctionDelegate(ProgressFunctionImpl);

        }
        public CurlDataCopier(Stream writeStream, Stream readStream, CancellationToken cancellationToken) :
            this(writeStream, cancellationToken) {

            this.readStream = readStream;

        }

        public WriteFunctionDelegate HeaderFunction => headerFunction;
        public ReadFunctionDelegate ReadFunction => readFunction;
        public WriteFunctionDelegate WriteFunction => writeFunction;
        public ProgressFunctionDelegate ProgressFunction => progressFunction;

        public void SetCallbacks(CurlEasyHandle easyHandle) {

            LibCurl.EasySetOpt(easyHandle, CurlOption.HeaderFunction, HeaderFunction);
            LibCurl.EasySetOpt(easyHandle, CurlOption.ReadFunction, ReadFunction);
            LibCurl.EasySetOpt(easyHandle, CurlOption.WriteFunction, WriteFunction);
            LibCurl.EasySetOpt(easyHandle, CurlOption.ProgessFunction, ProgressFunction);

        }

        // Private members

        private readonly Stream readStream;
        private readonly Stream writeStream;
        private readonly CancellationToken cancellationToken;
        private readonly WriteFunctionDelegate headerFunction;
        private readonly ReadFunctionDelegate readFunction;
        private readonly WriteFunctionDelegate writeFunction;
        private readonly ProgressFunctionDelegate progressFunction;

        private UIntPtr HeaderFunctionImpl(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            return WriteFunction(data, size, nmemb, userdata);

        }
        private UIntPtr ReadFunctionImpl(IntPtr buffer, UIntPtr size, UIntPtr nitems, IntPtr userdata) {

            if (readStream is null)
                return UIntPtr.Zero;

            int length = (int)size * (int)nitems;
            byte[] readBuffer = new byte[length];

            int bytesRead = readStream.Read(readBuffer, 0, length);

            Marshal.Copy(readBuffer, 0, buffer, length);

            return (UIntPtr)bytesRead;

        }
        private UIntPtr WriteFunctionImpl(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            int length = (int)size * (int)nmemb;
            byte[] buffer = new byte[length];

            Marshal.Copy(data, buffer, 0, length);

            if (writeStream != null)
                writeStream.Write(buffer, 0, length);

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
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Gsemac.Net.Curl {

    public class CurlCallbackHelper {

        // Public members

        public event WriteFunctionCallback Header;
        public event ReadFunctionCallback Read;
        public event WriteFunctionCallback Write;
        public event ProgressFunctionCallback Progress;

        public CurlCallbackHelper() {
        }
        public CurlCallbackHelper(Stream outputStream) :
            this(outputStream, CancellationToken.None) {
        }
        public CurlCallbackHelper(Stream outputStream, CancellationToken cancellationToken) {

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            this.outputStream = outputStream;
            this.cancellationToken = cancellationToken;

            Header += HeaderFunctionImpl;
            Read += ReadFunctionImpl;
            Write += WriteFunctionImpl;
            Progress += ProgressFunctionImpl;

        }
        public CurlCallbackHelper(Stream outputStream, Stream inputStream, CancellationToken cancellationToken) :
            this(outputStream, cancellationToken) {

            if (inputStream is null)
                throw new ArgumentNullException(nameof(inputStream));

            this.inputStream = inputStream;

        }

        public void SetCallbacks(CurlEasyHandle easyHandle) {

            LibCurl.EasySetOpt(easyHandle, CurlOption.HeaderFunction, Header);
            LibCurl.EasySetOpt(easyHandle, CurlOption.ReadFunction, Read);
            LibCurl.EasySetOpt(easyHandle, CurlOption.WriteFunction, Write);
            LibCurl.EasySetOpt(easyHandle, CurlOption.ProgessFunction, Progress);

        }

        // Private members

        private readonly Stream outputStream;
        private readonly Stream inputStream;
        private readonly CancellationToken cancellationToken = CancellationToken.None;

        private UIntPtr HeaderFunctionImpl(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            return WriteFunctionImpl(data, size, nmemb, userdata);

        }
        private UIntPtr ReadFunctionImpl(IntPtr buffer, UIntPtr size, UIntPtr nitems, IntPtr userdata) {

            if (inputStream is null)
                return UIntPtr.Zero;

            int length = (int)size * (int)nitems;
            byte[] readBuffer = new byte[length];

            int bytesRead = inputStream.Read(readBuffer, 0, length);

            Marshal.Copy(readBuffer, 0, buffer, length);

            return (UIntPtr)bytesRead;

        }
        private UIntPtr WriteFunctionImpl(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata) {

            int length = (int)size * (int)nmemb;
            byte[] buffer = new byte[length];

            Marshal.Copy(data, buffer, 0, length);

            if (outputStream != null)
                outputStream.Write(buffer, 0, length);

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
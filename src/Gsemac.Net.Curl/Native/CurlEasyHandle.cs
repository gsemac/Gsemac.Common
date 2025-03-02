using System;
using System.Runtime.InteropServices;

namespace Gsemac.Net.Curl.Native {

    public class CurlEasyHandle :
        SafeHandle {

        // Public members

        public override bool IsInvalid => handle == IntPtr.Zero;

        public CurlEasyHandle() :
            base(IntPtr.Zero, false) {
        }

        // Protected members

        protected override bool ReleaseHandle() {

            if (!IsClosed)
                LibCurl.EasyCleanup(this);

            return true;

        }
        protected override void Dispose(bool disposing) {

            if (disposing)
                ReleaseHandle();

            base.Dispose(disposing);

        }

    }

}
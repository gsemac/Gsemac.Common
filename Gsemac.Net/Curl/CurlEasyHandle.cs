using System;
using System.Runtime.InteropServices;

namespace Gsemac.Net.Curl {

    public class CurlEasyHandle :
        SafeHandle {

        public override bool IsInvalid => handle == IntPtr.Zero;

        public CurlEasyHandle() :
            base(IntPtr.Zero, false) {
        }

        protected override bool ReleaseHandle() {

            LibCurl.EasyCleanup(this);

            return true;

        }

    }

}
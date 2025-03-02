using System;
using System.Runtime.InteropServices;

namespace Gsemac.Net.Curl.Native {

    public class SListHandle :
          SafeHandle {

        public override bool IsInvalid => handle == IntPtr.Zero;

        public SListHandle() :
            base(IntPtr.Zero, false) {
        }

        protected override bool ReleaseHandle() {

            if (!IsClosed)
                LibCurl.SListFreeAll(this);

            return true;

        }
        protected override void Dispose(bool disposing) {

            if (disposing)
                ReleaseHandle();

            base.Dispose(disposing);

        }

    }

}
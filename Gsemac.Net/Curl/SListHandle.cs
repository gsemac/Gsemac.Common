using System;
using System.Runtime.InteropServices;

namespace Gsemac.Net.Curl {

    public class SListHandle :
          SafeHandle {

        public override bool IsInvalid => handle == IntPtr.Zero;

        public SListHandle() :
            base(IntPtr.Zero, false) {
        }

        protected override bool ReleaseHandle() {

            LibCurl.SListFreeAll(this);

            return true;

        }

    }

}
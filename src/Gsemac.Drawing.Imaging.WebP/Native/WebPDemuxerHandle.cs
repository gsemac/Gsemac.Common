using System;
using System.Runtime.InteropServices;

namespace Gsemac.Drawing.Imaging.Native {

    internal class WebPDemuxerHandle :
        SafeHandle {

        // Public members

        public override bool IsInvalid => handle == IntPtr.Zero;

        public WebPDemuxerHandle() :
            base(IntPtr.Zero, false) {
        }

        // Protected members

        protected override bool ReleaseHandle() {

            if (!IsClosed)
                LibWebPDemux.WebPDemuxDelete(this);

            return true;

        }
        protected override void Dispose(bool disposing) {

            if (disposing)
                ReleaseHandle();

            base.Dispose(disposing);

        }

    }

}
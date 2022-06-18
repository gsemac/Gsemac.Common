using Gsemac.Win32.Native;
using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    internal sealed class IconHandle :
        SafeHandle {

        public IconHandle(IntPtr invalidHandleValue, bool ownsHandle) :
            base(invalidHandleValue, ownsHandle) {
        }

        // Public members

        public override bool IsInvalid => handle == IntPtr.Zero;

        public IconHandle(IntPtr handle) :
            base(IntPtr.Zero, false) {

            SetHandle(handle);

        }

        // Protected members

        protected override bool ReleaseHandle() {

            bool result = User32.DestroyIcon(handle);

            SetHandleAsInvalid();

            return result;

        }
        protected override void Dispose(bool disposing) {

            if (disposing)
                ReleaseHandle();

            base.Dispose(disposing);

        }

    }

}
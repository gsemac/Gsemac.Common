using System;
using System.Runtime.InteropServices;

namespace Gsemac.Drawing.Imaging.Native {

    /// <summary>
    /// Data type used to describe 'raw' data, e.g., chunk data (ICC profile, metadata) and WebP compressed image data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct WebPData {

        public IntPtr bytes;
        public UIntPtr size;

    }

}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    [Flags]
    [SuppressMessage("Naming", "CA1712:Do not prefix enum values with type name", Justification = "The names are written to be identical to those in gdiplusenums.h.")]
    public enum EmfToWmfBitsFlags {
        EmfToWmfBitsFlagsDefault = 0x00000000,
        EmfToWmfBitsFlagsEmbedEmf = 0x00000001,
        EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,
        EmfToWmfBitsFlagsNoXORClip = 0x00000004,
    }

    public static class GdiPlus {

        // Public members

        public static uint GdipEmfToWmfBits(IntPtr hemf, uint cbData16, byte[] pData16, int iMapMode, EmfToWmfBitsFlags eFlags) {

            return GdipEmfToWmfBitsNative(hemf, cbData16, pData16, iMapMode, eFlags);

        }

        // Private members

        [DllImport("gdiplus", EntryPoint = "GdipEmfToWmfBits")]
        private static extern uint GdipEmfToWmfBitsNative(IntPtr hemf, uint cbData16, byte[] pData16, int iMapMode, EmfToWmfBitsFlags eFlags);

    }

}
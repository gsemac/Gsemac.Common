using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    [Flags]
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
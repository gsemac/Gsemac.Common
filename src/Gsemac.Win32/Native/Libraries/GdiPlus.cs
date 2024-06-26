﻿using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

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
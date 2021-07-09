using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    public static class Gdi32 {

        // Public members

        public static int MM_ISOTROPIC = 7;
        public static int MM_ANISOTROPIC = 8;

        public static IntPtr SetMetaFileBitsEx(uint cbBuffer, byte[] lpData) {

            return SetMetaFileBitsExNative(cbBuffer, lpData);

        }
        public static IntPtr CopyMetaFile(IntPtr hmf, string filename) {

            return CopyMetaFileNative(hmf, filename);

        }
        public static bool DeleteMetaFile(IntPtr hmf) {

            return DeleteMetaFileNative(hmf);

        }
        public static bool DeleteEnhMetaFile(IntPtr hmf) {

            return DeleteEnhMetaFileNative(hmf);

        }

        // Private members

        [DllImport("gdi32", EntryPoint = "SetMetaFileBitsEx")]
        private static extern IntPtr SetMetaFileBitsExNative(uint cbBuffer, byte[] lpData);
        [DllImport("gdi32", EntryPoint = "CopyMetaFile")]
        private static extern IntPtr CopyMetaFileNative(IntPtr hmf, string filename);
        [DllImport("gdi32", EntryPoint = "DeleteMetaFile")]
        private static extern bool DeleteMetaFileNative(IntPtr hmf);
        [DllImport("gdi32", EntryPoint = "DeleteEnhMetaFile")]
        private static extern bool DeleteEnhMetaFileNative(IntPtr hmf);

    }

}
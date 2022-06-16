using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    public static class Gdi32 {

        // Public members

        public static IntPtr CopyMetaFile(IntPtr hmf, string filename) {

            return CopyMetaFileNative(hmf, filename);

        }
        public static bool DeleteMetaFile(IntPtr hmf) {

            return DeleteMetaFileNative(hmf);

        }
        public static bool DeleteEnhMetaFile(IntPtr hmf) {

            return DeleteEnhMetaFileNative(hmf);

        }
        /// <summary>
        /// The <see cref="DeleteObject(IntPtr)"/> function deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns>A handle to a logical pen, brush, font, bitmap, region, or palette.</returns>
        public static bool DeleteObject(IntPtr hObject) {

            return DeleteObjectNative(hObject);

        }
        public static IntPtr SetMetaFileBitsEx(uint cbBuffer, byte[] lpData) {

            return SetMetaFileBitsExNative(cbBuffer, lpData);

        }

        // Private members

        [DllImport("gdi32", EntryPoint = "CopyMetaFile", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr CopyMetaFileNative(IntPtr hmf, string filename);
        [DllImport("gdi32", EntryPoint = "DeleteMetaFile")]
        private static extern bool DeleteMetaFileNative(IntPtr hmf);
        [DllImport("gdi32", EntryPoint = "DeleteEnhMetaFile")]
        private static extern bool DeleteEnhMetaFileNative(IntPtr hmf);
        [DllImport("gdi32", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObjectNative(IntPtr hObject);
        [DllImport("gdi32", EntryPoint = "SetMetaFileBitsEx")]
        private static extern IntPtr SetMetaFileBitsExNative(uint cbBuffer, byte[] lpData);

    }

}
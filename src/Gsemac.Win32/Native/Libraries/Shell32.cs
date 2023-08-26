using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class Shell32 {

        // Public members

        public static IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags) {

            return SHGetFileInfoNative(pszPath, dwFileAttributes, ref psfi, cbFileInfo, uFlags);

        }
        public static int SHGetKnownFolderPath(Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath) {

            return SHGetKnownFolderPathNative(rfid, dwFlags, hToken, out pszPath);

        } // Requires Windows Vista+

        // Private members

        [DllImport("shell32", EntryPoint = "SHGetFileInfo", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfoNative(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);
        [DllImport("shell32", EntryPoint = "SHGetKnownFolderPath", CharSet = CharSet.Unicode)]
        static extern int SHGetKnownFolderPathNative([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);

    }

}
using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class Shell32 {

        // Public members

        public static IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags) {

            return SHGetFileInfoNative(pszPath, dwFileAttributes, ref psfi, cbFileInfo, uFlags);

        }

        // Private members

        [DllImport("shell32", EntryPoint = "SHGetFileInfo", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfoNative(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);

    }

}
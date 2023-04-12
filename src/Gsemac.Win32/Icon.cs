using Gsemac.Win32.Native;
using System;
using System.Runtime.InteropServices;
using static Gsemac.Win32.Native.Defines;

#if NETFRAMEWORK
using System.Drawing;
#endif

namespace Gsemac.Win32 {

    public sealed class Icon :
        IDisposable {

        // Public members

        public SafeHandle Handle { get; }

        public static Icon FromFilePath(string filePath) {

            return FromFilePath(filePath, IconSize.Small);

        }
        public static Icon FromFilePath(string filePath, IconSize iconSize) {

            SHFILEINFO shInfo = new SHFILEINFO {
                szDisplayName = new string((char)0, MAX_PATH),
                szTypeName = new string((char)0, 80)
            };

            Shell32.SHGetFileInfo(filePath, FILE_ATTRIBUTE_NORMAL, ref shInfo, Marshal.SizeOf(shInfo), SHGFI_ICON | (int)iconSize | SHGFI_USEFILEATTRIBUTES);

            return FromHandle(shInfo.hIcon);

        }
        public static Icon FromHandle(IntPtr handle) {

            return new Icon(handle);

        }

#if NETFRAMEWORK

        public Bitmap ToBitmap() {

            return System.Drawing.Icon.FromHandle(Handle.DangerousGetHandle()).ToBitmap();

        }

#endif

        public void Dispose() {

            Handle.Dispose();

        }

        // Private members

        private Icon(IntPtr handle) {

            Handle = new IconHandle(handle);

        }

    }

}
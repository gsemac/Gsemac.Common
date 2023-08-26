using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEINFO {
        public IntPtr hIcon;
        public int iIcon;
        public int dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.MAX_PATH)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    public static partial class Constants {

        public const int SHGFI_LARGEICON = 0x0;
        public const int SHGFI_SMALLICON = 0x1;
        public const int SHGFI_ICON = 0x100;
        public const int SHGFI_USEFILEATTRIBUTES = 0x10;

    }

}
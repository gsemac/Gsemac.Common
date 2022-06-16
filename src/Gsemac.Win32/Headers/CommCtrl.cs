using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    [StructLayout(LayoutKind.Sequential)]
    public struct HDItemA {
        public uint mask;
        public int cxy;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszText;
        public IntPtr hbm;
        public int cchTextMax;
        public int fmt;
        public IntPtr lParam;
        public int iImage;
        public int iOrder;
        public uint type;
        public IntPtr pvFilter;
        public uint state;
    }

    public static partial class Defines {

        public const int HDF_SORTUP = 0x0400;
        public const int HDF_SORTDOWN = 0x0200;

        public const int HDI_WIDTH = 0x0001;
        public const int HDI_HEIGHT = HDI_WIDTH;
        public const int HDI_TEXT = 0x0002;
        public const int HDI_FORMAT = 0x0004;
        public const int HDI_LPARAM = 0x0008;
        public const int HDI_BITMAP = 0x0010;
        public const int HDI_IMAGE = 0x0020;
        public const int HDI_DI_SETITEM = 0x0040;
        public const int HDI_ORDER = 0x0080;
        public const int HDI_FILTER = 0x0100;
        public const int HDI_STATE = 0x2000;

        public const int HDM_FIRST = 0x1200;
        public const int HDM_GETITEM = HDM_FIRST + 11;
        public const int HDM_SETITEM = HDM_FIRST + 12;

        public const int LVM_FIRST = 0x1000;
        public const int LVM_GETHEADER = LVM_FIRST + 31;

    }

}
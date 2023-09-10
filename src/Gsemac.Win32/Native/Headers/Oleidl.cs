using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Gsemac.Win32.Native {

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DVTARGETDEVICE {

        [MarshalAs(UnmanagedType.U4)]
        public int tdSize;
        [MarshalAs(UnmanagedType.U2)]
        public short tdDriverNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdDeviceNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdPortNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdExtDevmodeOffset;
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]
        public byte[] tdData;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RECTL {

        public int left;
        public int top;
        public int right;
        public int bottom;

    }

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Style", "IDE1006:Naming Styles")]
    public sealed class tagLOGPALETTE {

        [MarshalAs(UnmanagedType.U2)]
        public ushort palVersion = 0;
        [MarshalAs(UnmanagedType.U2)]
        public ushort palNumEntries = 0;

    }

    [Guid("0000010d-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface IViewObject {

        void Draw([MarshalAs(UnmanagedType.U4)] int dwDrawAspect, int lindex, IntPtr pvAspect, DVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, RECTL lprcBounds, RECTL lprcWBounds, IntPtr pfnContinue, int dwContinue);
        int GetColorSet([MarshalAs(UnmanagedType.U4)] int dwDrawAspect, int lindex, IntPtr pvAspect, DVTARGETDEVICE ptd, IntPtr hicTargetDev, out tagLOGPALETTE ppColorSet);
        int Freeze([MarshalAs(UnmanagedType.U4)] int dwDrawAspect, int lindex, IntPtr pvAspect, out IntPtr pdwFreeze);
        int Unfreeze([MarshalAs(UnmanagedType.U4)] int dwFreeze);
        int SetAdvise([MarshalAs(UnmanagedType.U4)] int aspects, [MarshalAs(UnmanagedType.U4)] int advf, [MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);
        void GetAdvise([MarshalAs(UnmanagedType.LPArray)] out int[] paspects, [MarshalAs(UnmanagedType.LPArray)] out int[] advf, [MarshalAs(UnmanagedType.LPArray)] out IAdviseSink[] pAdvSink);

    }

}
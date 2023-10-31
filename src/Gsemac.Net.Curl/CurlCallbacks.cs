using System;
using System.Runtime.InteropServices;

namespace Gsemac.Net.Curl {

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate UIntPtr ReadFunctionCallback(IntPtr buffer, UIntPtr size, UIntPtr nItems, IntPtr userData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate UIntPtr WriteFunctionCallback(IntPtr data, UIntPtr size, UIntPtr nMemb, IntPtr userData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int ProgressFunctionCallback(IntPtr clientP, double dlTotal, double dlNow, double ulTotal, double ulNow);

}
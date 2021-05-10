using System;

namespace Gsemac.Net.Curl {

    public delegate UIntPtr ReadFunctionCallback(IntPtr buffer, UIntPtr size, UIntPtr nItems, IntPtr userData);
    public delegate UIntPtr WriteFunctionCallback(IntPtr data, UIntPtr size, UIntPtr nMemb, IntPtr userData);
    public delegate int ProgressFunctionCallback(IntPtr clientP, double dlTotal, double dlNow, double ulTotal, double ulNow);

}
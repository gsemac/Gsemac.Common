using System;

namespace Gsemac.Net.Curl {

    public delegate UIntPtr ReadFunctionCallback(IntPtr buffer, UIntPtr size, UIntPtr nitems, IntPtr userdata);
    public delegate UIntPtr WriteFunctionCallback(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata);
    public delegate int ProgressFunctionCallback(IntPtr clientp, double dltotal, double dlnow, double ultotal, double ulnow);

}
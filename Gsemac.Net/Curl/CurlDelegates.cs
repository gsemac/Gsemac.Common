using System;

namespace Gsemac.Net.Curl {

    public delegate UIntPtr WriteFunctionDelegate(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata);
    public delegate int ProgressFunctionDelegate(IntPtr clientp, double dltotal, double dlnow, double ultotal, double ulnow);

}
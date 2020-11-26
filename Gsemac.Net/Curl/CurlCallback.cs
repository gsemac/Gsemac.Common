using System;

namespace Gsemac.Net.Curl {

    public delegate UIntPtr WriteCallback(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata);
    public delegate int ProgressCallback(IntPtr clientp, double dltotal, double dlnow, double ultotal, double ulnow);

}
using System;

namespace Gsemac.Net.Curl.Native {

    [Flags]
    public enum CurlAuth :
        uint {

        None = 0,
        Basic = (uint)1 << 0,
        Digest = (uint)1 << 1,
        Negotiate = (uint)1 << 2,
        Ntlm = (uint)1 << 3,
        DigestIE = (uint)1 << 4,
        NtlmWB = (uint)1 << 5,
        Bearer = (uint)1 << 6,
        Only = (uint)1 << 31,
        Any = ~DigestIE,
        AnySafe = ~(Basic | DigestIE),

    }

}
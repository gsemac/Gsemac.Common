using System;

namespace Gsemac.Polyfills.System.Net {

    [Flags]
    public enum SecurityProtocolType {
        SystemDefault = 0,
        Ssl3 = 48,
        Tls = 192,
        Tls11 = 768,
        Tls12 = 3072,
        Tls13 = 12288,
    }

}
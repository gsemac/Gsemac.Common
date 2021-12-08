using System;

namespace Gsemac.IO {

    [Flags]
    public enum FileSignatureOptions {
        None = 0,
        CaseInsensitive = 1,
        Default = None,
    }

}
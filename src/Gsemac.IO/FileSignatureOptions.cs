using System;

namespace Gsemac.IO {

    [Flags]
    public enum FileSignatureOptions {
        None = 0,
        CaseInsensitive = 1,
        IgnoreLeadingWhiteSpace = 2,
        Default = None,
    }

}
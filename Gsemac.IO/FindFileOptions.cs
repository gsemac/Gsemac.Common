using System;

namespace Gsemac.IO {

    [Flags]
    public enum FindFileOptions {
        None = 0,
        Default = None,
        IgnoreExtension = 1,
        IgnoreCase = 2
    }

}
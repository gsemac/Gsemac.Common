using System;

namespace Gsemac.Utilities {

    [Flags]
    public enum FindFileOptions {
        None = 0,
        Default = None,
        IgnoreExtension = 1,
        IgnoreCase = 2
    }

}
using System;

namespace Gsemac.IO {

    [Flags]
    public enum PathDepthOptions {
        None = 0,
        IgnoreTrailingDirectorySeparators = 1,
        Default = IgnoreTrailingDirectorySeparators,
    }

}
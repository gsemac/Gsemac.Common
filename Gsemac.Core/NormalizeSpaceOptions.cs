using System;

namespace Gsemac.Core {

    [Flags]
    public enum NormalizeSpaceOptions {
        None = 0,
        Default = None,
        PreserveLineBreaks = 1,
        PreserveParagraphBreaks = 2
    }

}
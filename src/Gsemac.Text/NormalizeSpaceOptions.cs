﻿using System;

namespace Gsemac.Text {

    [Flags]
    public enum NormalizeSpaceOptions {
        None = 0,
        Default = None,
        PreserveLineBreaks = 1,
        PreserveParagraphBreaks = 2,
        Trim = 4,
    }

}
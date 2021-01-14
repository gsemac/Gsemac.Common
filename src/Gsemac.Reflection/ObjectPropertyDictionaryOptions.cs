﻿using System;

namespace Gsemac.Reflection {

    [Flags]
    public enum ObjectPropertyDictionaryOptions {
        None = 0,
        SkipReadOnlyProperties = 1,
        IncludeNestedProperties = 2,
        Default = None,
    }

}
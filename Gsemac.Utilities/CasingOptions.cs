using System;

namespace Gsemac.Utilities {

    [Flags]
    public enum CasingOptions {
        None = 0,
        CapitalizeRomanNumerals = 1,
        PreserveAcronyms = 2,
        Default = None
    }

}
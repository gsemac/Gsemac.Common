using System;

namespace Gsemac.Text {

    [Flags]
    public enum CaseConversionOptions {
        None = 0,
        CapitalizeRomanNumerals = 1,
        PreserveAcronyms = 2,
        Default = None
    }

}
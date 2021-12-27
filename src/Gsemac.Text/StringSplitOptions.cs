using System;

namespace Gsemac.Text {

    [Flags]
    public enum StringSplitOptions {
        None = 0,
        RemoveEmptyEntries = 1,
        TrimEntries = 2,
        AppendDelimiter = 4,
        PrependDelimiter = 8,
        RespectParentheses = 16,
        Default = None,
    }

}
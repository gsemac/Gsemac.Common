﻿namespace Gsemac.IO {

    public interface IReadLineOptions {

        bool BreakOnNewLine { get; }
        bool ConsumeDelimiter { get; }
        bool AllowEscapedDelimiters { get; }

    }

}
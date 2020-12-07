using System;

namespace Gsemac.IO {

    [Flags]
    public enum InvalidPathCharsOptions {
        None = 0,
        ReplaceInvalidPathChars = 1,
        ReplaceInvalidFileNameChars = 2,
        PreserveDirectoryStructure = 4,
        Default = ReplaceInvalidPathChars | ReplaceInvalidFileNameChars
    }

}
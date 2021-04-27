using System;

namespace Gsemac.IO {

    [Flags]
    public enum SanitizePathOptions {
        None = 0,
        StripInvalidPathChars = 1,
        StripInvalidFilenameChars = 2,
        PreserveDirectoryStructure = 4,
        StripRepeatedDirectorySeparators = 8,
        UseEquivalentValidPathChars = StripInvalidChars | 16,
        StripInvalidChars = StripInvalidPathChars | StripInvalidFilenameChars,
        Default = StripInvalidChars | PreserveDirectoryStructure
    }

}
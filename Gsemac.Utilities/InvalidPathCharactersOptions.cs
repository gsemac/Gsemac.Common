using System;

namespace Gsemac.Utilities {

    [Flags]
    public enum InvalidPathCharacterOptions {
        None = 0,
        Default = IncludeInvalidPathCharacters | IncludeInvalidFileNameCharacters,
        IncludeInvalidPathCharacters = 1,
        IncludeInvalidFileNameCharacters = 2,
        AllowPathSeparators = 4
    }

}
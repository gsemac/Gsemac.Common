using System;

namespace Gsemac.IO {

    [Flags]
    public enum InvalidPathCharacterOptions {
        None = 0,
        IncludeInvalidPathCharacters = 1,
        IncludeInvalidFileNameCharacters = 2,
        PreserveDirectoryStructure = 4,
        Default = IncludeInvalidPathCharacters | IncludeInvalidFileNameCharacters
    }

}
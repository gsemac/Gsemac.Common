namespace Gsemac.IO {

    public interface ISanitizePathOptions {

        bool StripInvalidPathChars { get; }
        bool StripInvalidFileNameChars { get; }
        bool PreserveDirectoryStructure { get; }
        bool StripRepeatedDirectorySeparators { get; }
        bool NormalizeDirectorySeparators { get; }
        bool UseEquivalentValidPathChars { get; }

    }

}
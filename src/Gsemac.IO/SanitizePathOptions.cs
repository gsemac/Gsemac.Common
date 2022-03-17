using System;

namespace Gsemac.IO {

    public class SanitizePathOptions :
        ISanitizePathOptions {

        // Public members

        public bool StripInvalidPathChars { get; set; } = true;
        public bool StripInvalidFilenameChars { get; set; } = true;
        public bool PreserveDirectoryStructure { get; set; } = true;
        public bool StripRepeatedDirectorySeparators { get; set; } = false;
        public bool NormalizeDirectorySeparators { get; set; } = true;
        public bool UseEquivalentValidPathChars {
            get => GetUseEquivalentValidPathChars();
            set => SetUseEquivalentValidPathChars(value);
        }

        public static SanitizePathOptions Default => new SanitizePathOptions();
        public static SanitizePathOptions StripInvalidChars => new SanitizePathOptions(None) {
            StripInvalidPathChars = true,
            StripInvalidFilenameChars = true,
        };
        public static SanitizePathOptions None => new SanitizePathOptions() {
            StripInvalidPathChars = false,
            StripInvalidFilenameChars = false,
            PreserveDirectoryStructure = false,
            StripRepeatedDirectorySeparators = false,
            NormalizeDirectorySeparators = false,
            UseEquivalentValidPathChars = false,
        };

        public SanitizePathOptions() { }
        public SanitizePathOptions(ISanitizePathOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            StripInvalidPathChars = options.StripInvalidPathChars;
            StripInvalidFilenameChars = options.StripInvalidFilenameChars;
            PreserveDirectoryStructure = options.PreserveDirectoryStructure;
            StripRepeatedDirectorySeparators = options.StripRepeatedDirectorySeparators;
            NormalizeDirectorySeparators = options.NormalizeDirectorySeparators;
            UseEquivalentValidPathChars = options.UseEquivalentValidPathChars;

        }

        // Private members

        private bool useEquivalentValidPathChars = false;

        private bool GetUseEquivalentValidPathChars() {

            return useEquivalentValidPathChars;

        }
        private void SetUseEquivalentValidPathChars(bool value) {

            useEquivalentValidPathChars = value;

            if (useEquivalentValidPathChars) {

                StripInvalidPathChars = true;
                StripInvalidFilenameChars = true;

            }

        }


    }

}
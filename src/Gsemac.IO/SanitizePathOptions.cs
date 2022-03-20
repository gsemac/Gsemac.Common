using System;

namespace Gsemac.IO {

    public class SanitizePathOptions :
        ISanitizePathOptions {

        // Public members

        public bool StripInvalidPathChars { get; set; }
        public bool StripInvalidFilenameChars { get; set; }
        public bool PreserveDirectoryStructure { get; set; }
        public bool StripRepeatedDirectorySeparators { get; set; }
        public bool NormalizeDirectorySeparators { get; set; }
        public bool UseEquivalentValidPathChars {
            get => GetUseEquivalentValidPathChars();
            set => SetUseEquivalentValidPathChars(value);
        }

        public static SanitizePathOptions Default => CreateDefault();
        public static SanitizePathOptions StripInvalidChars => new SanitizePathOptions() {
            StripInvalidPathChars = true,
            StripInvalidFilenameChars = true,
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

        private static SanitizePathOptions CreateDefault() {

            return new SanitizePathOptions() {
                StripInvalidPathChars = true,
                StripInvalidFilenameChars = true,
                PreserveDirectoryStructure = true,
                NormalizeDirectorySeparators = true,
            };

        }


    }

}
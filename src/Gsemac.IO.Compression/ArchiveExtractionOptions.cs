namespace Gsemac.IO.Compression {

    public sealed class ArchiveExtractionOptions :
        IArchiveExtractionOptions {

        // Public members

        public bool ExtractToNewFolder { get; set; } = false;
        public string OutputDirectoryPath { get; set; }
        public string Password { get; set; }

        // Private members

        public static ArchiveExtractionOptions Default => new ArchiveExtractionOptions();

    }

}
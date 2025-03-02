namespace Gsemac.IO.Compression.SevenZip {

    public sealed class SevenZipProcessArchiveFactoryOptions :
        ISevenZipProcessArchiveFactoryOptions {

        // Public members

        public string SevenZipDirectoryPath { get; set; } = string.Empty;

        public static SevenZipProcessArchiveFactoryOptions Default => new SevenZipProcessArchiveFactoryOptions();

    }

}
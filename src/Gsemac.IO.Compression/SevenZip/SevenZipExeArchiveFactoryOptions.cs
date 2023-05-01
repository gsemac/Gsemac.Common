namespace Gsemac.IO.Compression.SevenZip {

    public sealed class SevenZipExeArchiveFactoryOptions :
        ISevenZipExeArchiveFactoryOptions {

        // Public members

        public string SevenZipDirectoryPath { get; set; } = string.Empty;

        public static SevenZipExeArchiveFactoryOptions Default => new SevenZipExeArchiveFactoryOptions();

    }

}
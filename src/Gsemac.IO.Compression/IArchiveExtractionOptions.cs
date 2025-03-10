namespace Gsemac.IO.Compression {

    public interface IArchiveExtractionOptions {

        bool ExtractToNewFolder { get; }
        string OutputDirectoryPath { get; }
        string Password { get; }

    }

}
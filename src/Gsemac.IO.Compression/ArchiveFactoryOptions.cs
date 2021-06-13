namespace Gsemac.IO.Compression {

    public class ArchiveFactoryOptions :
        IArchiveFactoryOptions {

        public string SevenZipDirectoryPath { get; set; }
        public string WinrarDirectoryPath { get; set; }

        public static ArchiveFactoryOptions Default => new ArchiveFactoryOptions();

    }

}
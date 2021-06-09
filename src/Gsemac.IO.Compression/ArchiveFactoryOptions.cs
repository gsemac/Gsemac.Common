namespace Gsemac.IO.Compression {

    public class ArchiveFactoryOptions :
        IArchiveFactoryOptions {

        public string SevenZipExecutablePath { get; set; }
        public string WinRarExecutablePath { get; set; }

        public static ArchiveFactoryOptions Default => new ArchiveFactoryOptions();

    }

}
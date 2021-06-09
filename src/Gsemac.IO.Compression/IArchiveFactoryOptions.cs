namespace Gsemac.IO.Compression {

    public interface IArchiveFactoryOptions {

        string SevenZipExecutablePath { get; }
        string WinRarExecutablePath { get; }

    }

}
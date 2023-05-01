namespace Gsemac.IO.Compression.Winrar {

    public sealed class WinrarExeArchiveFactoryOptions :
        IWinrarExeArchiveFactoryOptions {

        // Public members

        public string WinrarDirectoryPath { get; set; } = string.Empty;

        public static WinrarExeArchiveFactoryOptions Default => new WinrarExeArchiveFactoryOptions();

    }

}
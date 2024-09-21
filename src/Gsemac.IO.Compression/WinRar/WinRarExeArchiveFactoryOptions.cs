namespace Gsemac.IO.Compression.WinRar {

    public sealed class WinRarExeArchiveFactoryOptions :
        IWinRarExeArchiveFactoryOptions {

        // Public members

        public string WinRarDirectoryPath { get; set; } = string.Empty;

        public static WinRarExeArchiveFactoryOptions Default => new WinRarExeArchiveFactoryOptions();

    }

}
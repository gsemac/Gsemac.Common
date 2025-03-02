namespace Gsemac.IO.Compression.WinRar {

    public sealed class WinRarProcessArchiveFactoryOptions :
        IWinRarProcessArchiveFactoryOptions {

        // Public members

        public string WinRarDirectoryPath { get; set; } = string.Empty;

        public static WinRarProcessArchiveFactoryOptions Default => new WinRarProcessArchiveFactoryOptions();

    }

}
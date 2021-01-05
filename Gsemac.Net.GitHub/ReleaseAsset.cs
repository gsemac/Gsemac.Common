namespace Gsemac.Net.GitHub {

    internal class ReleaseAsset :
         IReleaseAsset {

        public string Name { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;

        public bool IsSourceArchive => DownloadUrl.Contains("/archive/");

    }

}
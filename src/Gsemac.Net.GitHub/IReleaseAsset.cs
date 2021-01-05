namespace Gsemac.Net.GitHub {

    public interface IReleaseAsset {

        string Name { get; }
        string DownloadUrl { get; }

        bool IsSourceArchive { get; }

    }

}
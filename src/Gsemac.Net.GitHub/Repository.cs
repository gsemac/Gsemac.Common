namespace Gsemac.Net.GitHub {

    internal class Repository :
        IRepository {

        public IRepositoryUrl Url { get; set; }
        public string Owner => Url.Owner;
        public string Name => Url.RepositoryName;

        public string DownloadUrl { get; set; }

    }

}
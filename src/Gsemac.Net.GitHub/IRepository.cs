namespace Gsemac.Net.GitHub {

    public interface IRepository {

        IRepositoryUrl Url { get; }
        string Owner { get; }
        string Name { get; }
        string DownloadUrl { get; }

    }

}
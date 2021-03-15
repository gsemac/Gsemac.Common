namespace Gsemac.Net.GitHub {

    public interface IRepository {

        string DefaultBranchName { get; }
        string Url { get; }
        string Name { get; }
        string Owner { get; }
        string Tree { get; }

        string ArchiveUrl { get; }
        string ReleasesUrl { get; }

    }

}
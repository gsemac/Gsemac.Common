namespace Gsemac.Net.GitHub {

    public interface IRepositoryUrl {

        string Owner { get; }
        string RepositoryName { get; }
        string BranchName { get; }

        string CodeUrl { get; }
        string ReleasesUrl { get; }

    }

}
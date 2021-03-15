namespace Gsemac.Net.GitHub {

    public interface IGitHubUrl {

        string Owner { get; }
        string Path { get; }
        string RepositoryName { get; }
        string Tree { get; }

    }

}
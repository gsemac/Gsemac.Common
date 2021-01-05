using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.GitHub.Extensions {

    public static class GithubClientExtensions {

        public static IEnumerable<IRelease> GetReleases(this IGitHubClient gitHubClient, string repositoryUrl, int numberOfReleases) {

            return gitHubClient.GetReleases(new RepositoryUrl(repositoryUrl), numberOfReleases);

        }

        public static IRelease GetLatestRelease(this IGitHubClient gitHubClient, string repositoryUrl) {

            return GetLatestRelease(gitHubClient, new RepositoryUrl(repositoryUrl));

        }
        public static IRelease GetLatestRelease(this IGitHubClient gitHubClient, IRepositoryUrl repositoryUrl) {

            return gitHubClient.GetReleases(repositoryUrl, 1).FirstOrDefault();

        }

    }

}